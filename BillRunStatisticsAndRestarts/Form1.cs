using Newtonsoft.Json;
using System.Data.SqlClient;
using System.Diagnostics.Metrics;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Windows.Forms;

namespace BillRunStatisticsAndRestarts
{
    public partial class Form1 : Form
    {


        public static readonly string FolderPath = "C:\\Temp\\BillRunMetrics";
        public static readonly string ServiceRestartsDir = Path.Combine(FolderPath, "ServiceRestarts");
        public static readonly string BillRunStatsFile = "Metrics.csv";
        public static readonly string BillRunStatsWithRestartsFileName = "MetricsWithRestarts.csv";

        public string CurrentClient { get; set; }
        public string ClientDirectory => Path.Combine(FolderPath, CurrentClient);
        public string? CurrrentClientsAppServer => ClientsAndAppServers.Where(x => x.ClientName.ToUpper().Equals(CurrentClient, StringComparison.OrdinalIgnoreCase)).Select(x => x.AppServerName.ToUpper()).SingleOrDefault();
        public string ClientsAppServerShort => StringFunctions.Left(CurrrentClientsAppServer, 5).ToUpper();
        public string StartDate => StartDateTimePicker.Value.ToString("yyyy-MM-dd");

        public List<ClientAndAppServer> ClientsAndAppServers = new()
        {
            new ClientAndAppServer("HUNTER", "app11.core00.rev.io"),
            new ClientAndAppServer("NUWAVE", "app11.core00.rev.io"),
            new ClientAndAppServer("SKYSWITCH", "app11.core00.rev.io"),
            new ClientAndAppServer("CLEARRATE", "app12.core00.rev.io"),
            new ClientAndAppServer("SOTELSYSTEMS", "app12.core00.rev.io"),
            new ClientAndAppServer("CALLTOWER", "app13.core00.rev.io"),
            new ClientAndAppServer("SPECTROTEL", "app13.core00.rev.io"),
            new ClientAndAppServer("CCI", "app14.core00.rev.io"),
            new ClientAndAppServer("MACHNETWORKS", "app15.core00.rev.io"),
            new ClientAndAppServer("BBOSOLUTIONS", "app16.core00.rev.io"),
            new ClientAndAppServer("TOUCHTONE", "app18.core00.rev.io")
        };

        public List<string> BillRunStatisticsFields = new()
        {
            "Statement_Create_Batch_ID",
            "Bill_Run_Date",
            "Bill_Run_Completed_Date",
            "Bill_Run_Total_Duration_Minutes",
            "Bill_Creation_Duration_Minutes",
            "Print_Batch_Duration_Minutes",
            "MRC_Duration_Minutes",
            "MRC_Start_Date",
            "MRC_End_Date",
            "MRC_Customer_Count",
            "MRC_Count",
            "MRCs_Per_Minute",
            "MRC_To_Bill_Delay_Minutes",
            "Bill_Creation_Start_Date",
            "Bill_Creation_End_Date",
            "Bill_Creation_Count",
            "Bill_Creation_Non_Child_Customer_Count",
            "Bill_Creation_Child_Customer_Count",
            "Bill_Creation_Per_Minute",
            "Print_Batch_Count",
            "Print_Batch_First_Start_Date",
            "Print_Batch_Last_End_Date",
        };

        public List<string> NewFieldsToAddToStats = new()
        {
            "RecurringBillingRestarted",
            "CreateStatementsRestarted"
        };

        public CancellationTokenSource cts { get; set; }

        public Form1()
        {
            InitializeComponent();
            cts = new CancellationTokenSource();
            checkedListBox1.Items.AddRange(ClientsAndAppServers.Select(x => x.ClientName).OrderBy(name => name).ToArray());
            AddMessage("Ready to run!");
        }

        private async void Button1_Click(object sender, EventArgs e)
        {
            cts = new CancellationTokenSource();

            foreach (var checkedItem in checkedListBox1.CheckedItems)
            {
                CurrentClient = (string)checkedItem;

                if (string.IsNullOrEmpty(CurrrentClientsAppServer))
                {
                    AddMessage("Could not find client!");
                    return;
                }

                if (!Directory.Exists(ClientDirectory))
                {
                    Directory.CreateDirectory(ClientDirectory);
                    AddMessage($"Directory created: {ClientDirectory}");
                }
                AddMessage($"Client Directory set to {ClientDirectory}");

                var path = Path.Combine(ClientDirectory, BillRunStatsFile);
                AddMessage($"Reading bill run data from {path}");
                var metrics = await ReadOriginalMetrics(path, cts.Token);

                var serviceRestarts = await ReadServiceRestartData(cts.Token);

                var updatedMetrics = await DetermineBillRunsImpactedByServiceRestarts(metrics, serviceRestarts, cts.Token);
                await WriteFile(updatedMetrics, cts.Token);
            }
            AddMessage("Complete.");
        }

        private async Task<List<BillRunMetricsResults>> ReadOriginalMetrics(string path, CancellationToken ct)
        {
            AddMessage("Reading Bill Run Metrics...");
            var results = new List<BillRunMetricsResults>();

            if (File.Exists(path))
            {
                try
                {
                    using var reader = new StreamReader(path);

                    string? line = await reader.ReadLineAsync();

                    while (line != null && !ct.IsCancellationRequested)
                    {
                        if (ct.IsCancellationRequested) break;
                        string[] fields = line.Split(",");

                        BillRunMetricsResults result = new()
                        {
                            Statement_Create_Batch_ID = fields[0],
                            Bill_Run_Date = fields[1],
                            Bill_Run_Completed_Date = fields[2],
                            Bill_Run_Total_Duration_Minutes = fields[3],
                            Bill_Creation_Duration_Minutes = fields[4],
                            Print_Batch_Duration_Minutes = fields[5],
                            MRC_Duration_Minutes = fields[6],
                            MRC_Start_Date = DateTime.TryParse(fields[7], out var mrcStart) ? mrcStart : null,
                            MRC_End_Date = DateTime.TryParse(fields[8], out var mrcEnd) ? mrcEnd : null,
                            MRC_Customer_Count = fields[9],
                            MRC_Count = fields[10],
                            MRCs_Per_Minute = fields[11],
                            MRC_To_Bill_Delay_Minutes = fields[12],
                            Bill_Creation_Start_Date = DateTime.TryParse(fields[13], out var billCreateStart) ? billCreateStart : null,
                            Bill_Creation_End_Date = DateTime.TryParse(fields[14], out var billCreateEnd) ? billCreateEnd : null,
                            Bill_Creation_Count = fields[15],
                            Bill_Creation_Non_Child_Customer_Count = fields[16],
                            Bill_Creation_Child_Customer_Count = fields[17],
                            Bill_Creation_Per_Minute = fields[18],
                            Print_Batch_Count = fields[19],
                            Print_Batch_First_Start_Date = fields[20],
                            Print_Batch_Last_End_Date = fields[21],
                        };

                        results.Add(result);
                        line = await reader.ReadLineAsync();
                    }
                }
                catch (Exception ex)
                {
                    AddMessage(ex.Message);
                    return results;
                }
            }
            else
            {
                // We're gonna read from the DB! 
                try
                {
                    var connString = GetClientConnectionString(CurrentClient);
                    if (string.IsNullOrEmpty(connString)) return results;

                    using var sqlConnection = new SqlConnection(connString);
                    sqlConnection.Open();

                    using var sp = new SqlCommand("og.bill_run_metrics", sqlConnection);
                    sp.CommandType = System.Data.CommandType.StoredProcedure;
                    sp.Parameters.AddWithValue("@start_date", StartDate);
                    sp.Parameters.AddWithValue("@end_date", null);
                    sp.Parameters.AddWithValue("@include_mrcs", 1);

                    using var sqlReader = await sp.ExecuteReaderAsync(ct);
                    using var writer = new StreamWriter(path);

                    while (sqlReader.Read())
                    {
                        var billRunMetric = new BillRunMetricsResults();

                        billRunMetric.Statement_Create_Batch_ID = Convert.ToString(sqlReader[0]);
                        billRunMetric.Bill_Run_Date = Convert.ToString(sqlReader[1]);
                        billRunMetric.Bill_Run_Completed_Date = Convert.ToString(sqlReader[2]);
                        billRunMetric.Bill_Run_Total_Duration_Minutes = Convert.ToString(sqlReader[3]);
                        billRunMetric.Bill_Creation_Duration_Minutes = Convert.ToString(sqlReader[4]);
                        billRunMetric.Print_Batch_Duration_Minutes = Convert.ToString(sqlReader[5]);
                        billRunMetric.MRC_Duration_Minutes = Convert.ToString(sqlReader[6]);
                        billRunMetric.MRC_Start_Date = DateTime.TryParse(Convert.ToString(sqlReader[7]), out var mrcStart) ? mrcStart : null;
                        billRunMetric.MRC_End_Date = DateTime.TryParse(Convert.ToString(sqlReader[8]), out var mrcEnd) ? mrcEnd : null;
                        billRunMetric.MRC_Customer_Count = Convert.ToString(sqlReader[9]);
                        billRunMetric.MRC_Count = Convert.ToString(sqlReader[10]);
                        billRunMetric.MRCs_Per_Minute = Convert.ToString(sqlReader[11]);
                        billRunMetric.MRC_To_Bill_Delay_Minutes = Convert.ToString(sqlReader[12]);
                        billRunMetric.Bill_Creation_Start_Date = DateTime.TryParse(Convert.ToString(sqlReader[13]), out var billCreateStart) ? billCreateStart : null;
                        billRunMetric.Bill_Creation_End_Date = DateTime.TryParse(Convert.ToString(sqlReader[14]), out var billCreateEnd) ? billCreateEnd : null;
                        billRunMetric.Bill_Creation_Count = Convert.ToString(sqlReader[15]);
                        billRunMetric.Bill_Creation_Non_Child_Customer_Count = Convert.ToString(sqlReader[16]);
                        billRunMetric.Bill_Creation_Child_Customer_Count = Convert.ToString(sqlReader[17]);
                        billRunMetric.Bill_Creation_Per_Minute = Convert.ToString(sqlReader[18]);
                        billRunMetric.Print_Batch_Count = Convert.ToString(sqlReader[19]);
                        billRunMetric.Print_Batch_First_Start_Date = Convert.ToString(sqlReader[20]);
                        billRunMetric.Print_Batch_Last_End_Date = Convert.ToString(sqlReader[21]);

                        await writer.WriteLineAsync(billRunMetric.GetCSVLine());
                        results.Add(billRunMetric);
                    }
                }
                catch (Exception ex)
                {
                    AddMessage(ex.Message);
                    return results;
                }
            }

            return results;
        }

        //// If we can get data out of datadog, these indexes will almost definitely need to change
        //private readonly int AppServerIndex = 0;
        //private readonly int RecurringBillingRestartTimeIndex = 1;
        //private readonly int CreateStatementsRestartTimeIndex = 2;

        private async Task<List<ServiceRestartInfo>> ReadServiceRestartData(CancellationToken ct)
        {
            AddMessage("Reading service restart data...");
            var dataDogs = new List<ServiceRestartInfo>();
            string[] jsonFiles = Directory.GetFiles(ServiceRestartsDir, "*.json");

            foreach (string jsonFile in jsonFiles)
            {
                AddMessage($"Reading {jsonFile}...");
                string jsonContent = await File.ReadAllTextAsync(jsonFile, ct);
                var eventList = JsonConvert.DeserializeObject<DataDogEvents>(jsonContent);

                if (eventList == null)
                    continue;

                foreach (var ev in eventList.Events)
                {
                    // Recurring Billing or Create Statements? 
                    var host = ev.host;
                    var restartDate = DateTimeOffset.FromUnixTimeSeconds(ev.date_happened).LocalDateTime;
                    var restartDate2 = DateTimeOffset.FromUnixTimeSeconds(ev.date_happened).DateTime;

                    var serviceRestartInfo = new ServiceRestartInfo()
                    {
                        AppServer = host,
                    };

                    if (ev.monitor_groups.Any(x => x.ToUpper().Contains("RECURRINGBILLING"))
                        || ev.tags.Any(y => y.ToUpper().Contains("OC.SERVICES.H2O.RECURRINGBILLING")))
                    {
                        serviceRestartInfo.MRCRestartTime = restartDate;
                    }
                    else if (ev.monitor_groups.Any(x => x.ToUpper().Contains("CREATESTATEMENTS"))
                        || ev.tags.Any(y => y.ToUpper().Contains("OC.SERVICES.H2O.CREATESTATEMENTS")))
                    {
                        serviceRestartInfo.CreateStatementRestartTime = restartDate;
                    }

                    dataDogs.Add(serviceRestartInfo);
                }
            }
            return dataDogs;
        }
        public async Task<List<BillRunMetricsResults>> DetermineBillRunsImpactedByServiceRestarts(List<BillRunMetricsResults> metrics, List<ServiceRestartInfo> restarts, CancellationToken ct)
        {
            AddMessage("Determining which bill runs required or were interrupted by service restarts...");

            int mrcRestarts = 0, csRestarts = 0;
            foreach (var metric in metrics)
            {
                var applicableRestarts = restarts.Where(r => r.AppServer.Equals(CurrrentClientsAppServer) || r.AppServer.Equals(ClientsAppServerShort));

                if (metric.MRC_Start_Date.HasValue && metric.MRC_End_Date.HasValue)
                {
                    if (applicableRestarts.Any(r => metric.MRC_Start_Date <= r.MRCRestartTime && r.MRCRestartTime <= metric.MRC_End_Date))
                    {
                        metric.RecurringBillingRestarted = "TRUE";
                        mrcRestarts++;
                    }
                }

                if (metric.Bill_Creation_Start_Date.HasValue && metric.Bill_Creation_End_Date.HasValue)
                {
                    if (applicableRestarts.Any(r => metric.Bill_Creation_Start_Date <= r.CreateStatementRestartTime && r.CreateStatementRestartTime <= metric.Bill_Creation_End_Date))
                    {
                        metric.CreateStatementsRestarted = "TRUE";
                        csRestarts++;
                    }
                }

                if (ct.IsCancellationRequested) break;
            }

            AddMessage($"Bill runs impacted by CreateStatements restarts: {csRestarts}.");
            AddMessage($"Bill runs impacted by RecurringBilling restarts: {mrcRestarts}.");
            return metrics;
        }
        private async Task WriteFile(List<BillRunMetricsResults> metrics, CancellationToken ct)
        {
            var path = Path.Combine(ClientDirectory, BillRunStatsWithRestartsFileName);
            AddMessage($"Writing new file to {path}");
            using (var writer = new StreamWriter(path))
            {
                // write header
                var header = string.Join(",", BillRunStatisticsFields.Union(NewFieldsToAddToStats));
                await writer.WriteLineAsync(header);

                foreach (var metric in metrics)
                {
                    string line = metric.GetCSVLine();
                    await writer.WriteLineAsync(line);

                    if (ct.IsCancellationRequested) break;
                }
            }
        }



        private async void CancelButton_Click(object sender, EventArgs e)
        {
            // This usually runs too fast to even click the button, but if it ever gets hung up for whatever reason, this should work. 
            AddMessage("Cancel clicked.");
            cts.Cancel();
        }

        //private List<string> Messages { get; set; } = new List<string>();
        private void AddMessage(string message)
        {
            var timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            var messageWithTimestamp = $"{timestamp}: {message}";
            //Messages.Add(messageWithTimestamp);
            MessagesListBox.Items.Add(messageWithTimestamp);
            MessagesListBox.SelectedIndex = MessagesListBox.Items.Count - 1;
        }


        private const string Global01CS = "Initial Catalog=OVERGROUP;Persist Security Info=False;Integrated Security=SSPI;Data Source=global01.acs.overgroup.com";
        public void TestConnectToDatabase(string connectionString, string dbNameForMessage = "the database")
        {
            using SqlConnection connection = new(connectionString);
            try
            {
                connection.Open();

                // Connection is established successfully
                var message = $"Connected to {dbNameForMessage}!";
                Console.WriteLine(message);
                AddMessage(message);
                // Perform database operations as needed

                connection.Close();
            }
            catch (Exception ex)
            {
                // Handle any potential errors
                var errormess = $"Error connecting to {dbNameForMessage}: " + ex.Message;
                Console.WriteLine(errormess);
                AddMessage(errormess);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            TestConnectToDatabase(Global01CS, "Global01");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            using SqlConnection connection = new(Global01CS);
            try
            {
                connection.Open();

                // Connection is established successfully
                var message = "Connected to Global01!";
                Console.WriteLine(message);
                AddMessage(message);
                // Perform database operations as needed
                foreach (var item in checkedListBox1.CheckedItems)
                {
                    var clientCode = (string)item;

                    var command = new SqlCommand($"SELECT H2O_Conn_String from tblClient where Code = '{clientCode}'", connection);
                    var h2oConn = command.ExecuteScalar()?.ToString();

                    TestConnectToDatabase(h2oConn, clientCode);
                }
            }
            catch (Exception ex)
            {
                // Handle any potential errors
                var errormess = "Error connecting to the database: " + ex.Message;
                Console.WriteLine(errormess);
                AddMessage(errormess);
            }
        }

        private string GetClientConnectionString(string clientCode)
        {
            var returnConnString = "";
            using SqlConnection connection = new(Global01CS);
            try
            {
                connection.Open();
                var command = new SqlCommand($"SELECT H2O_Conn_String from tblClient where Code = '{clientCode}'", connection);
                returnConnString = command.ExecuteScalar()?.ToString();
            }
            catch (Exception ex)
            {
                AddMessage("Error connecting to the database: " + ex.Message);
            }
            return returnConnString;
        }
    }
}