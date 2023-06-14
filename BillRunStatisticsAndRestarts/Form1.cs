using Newtonsoft.Json;
using OfficeOpenXml;
using System.Data.SqlClient;

namespace BillRunStatisticsAndRestarts
{
    public partial class Form1 : Form
    {
        public static readonly string FolderPath = "C:\\Temp\\BillRunMetrics";
        public string ServiceRestartsDir => Path.Combine(FolderPath, "ServiceRestarts");
        public static readonly string BillRunStatsFile = "Metrics.csv";
        public string BillRunStatsWithRestartsFileName => CurrentClient + "MetricsWithRestarts.csv";

        public string CurrentClient { get; set; } = "";
        public string ClientDirectory => Path.Combine(FolderPath, CurrentClient);
        public string CurrrentClientsAppServer => ClientsAndAppServers.Where(x => x.ClientName.ToUpper().Equals(CurrentClient.ToUpper())).Select(x => x.AppServerName.ToUpper()).Single();
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

        public CancellationTokenSource CTS { get; set; }
        public List<string> Messages { get; set; } = new List<string>();
        public Dictionary<string, List<BillRunMetricsResults>> ClientBillRunMetrics { get; set; } = new Dictionary<string, List<BillRunMetricsResults>>();

        public Form1()
        {
            InitializeComponent();
            CTS = new CancellationTokenSource();

            checkedListBox1.Items.AddRange(ClientsAndAppServers.Select(x => x.ClientName).OrderBy(name => name).ToArray());
            for (int i = 0; i < checkedListBox1.Items.Count; i++)
                checkedListBox1.SetItemCheckState(i, CheckState.Indeterminate);

            // Technically this is an internal tool. Does that count?
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            AddMessage("Ready to run!");
        }
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            string logPath = Path.Combine(FolderPath, $"log.txt"); // {DateTime.Now:yyyyMMdd_HHmmss}.txt");
            File.WriteAllText(logPath, string.Join(Environment.NewLine, Messages.ToArray()));
        }

        private async void Button1_Click(object sender, EventArgs e)
        {
            CTS = new CancellationTokenSource();
            var createdFiles = new List<string>();

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
                var metrics = await ReadOriginalMetrics(path, CTS.Token);
                if (metrics == null || !metrics.Any())
                {
                    AddMessage("Could not gather original bill run metrics.");
                    continue;
                }
                var serviceRestarts = await ReadServiceRestartData(CTS.Token);

                var updatedMetrics = await DetermineBillRunsImpactedByServiceRestarts(metrics, serviceRestarts, CTS.Token);

                ClientBillRunMetrics.Remove(CurrentClient);
                ClientBillRunMetrics.Add(CurrentClient, updatedMetrics);

                var filePath = await WriteClientCSVFile(updatedMetrics, CTS.Token);
                createdFiles.Add(filePath);
            }

            if (CreateCombinedFileCheckBox.Checked)
            {
                await CreateCombinedExcelFile(createdFiles);
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
                            Statement_Create_Batch_ID = ParserFunctions.GetInt(fields[0]),
                            Bill_Run_Date = fields[1],
                            Bill_Run_Completed_Date = fields[2],
                            Bill_Run_Total_Duration_Minutes = ParserFunctions.GetDouble(fields[3]),
                            Bill_Creation_Duration_Minutes = ParserFunctions.GetDouble(fields[4]),
                            Print_Batch_Duration_Minutes = ParserFunctions.GetDouble(fields[5]),
                            MRC_Duration_Minutes = ParserFunctions.GetDouble(fields[6]),
                            MRC_Start_Date = DateTime.TryParse(fields[7], out var mrcStart) ? mrcStart : null,
                            MRC_End_Date = DateTime.TryParse(fields[8], out var mrcEnd) ? mrcEnd : null,
                            MRC_Customer_Count = ParserFunctions.GetInt(fields[9]),
                            MRC_Count = ParserFunctions.GetInt(fields[10]),
                            MRCs_Per_Minute = ParserFunctions.GetDouble(fields[11]),
                            MRC_To_Bill_Delay_Minutes = ParserFunctions.GetDouble(fields[12]),
                            Bill_Creation_Start_Date = DateTime.TryParse(fields[13], out var billCreateStart) ? billCreateStart : null,
                            Bill_Creation_End_Date = DateTime.TryParse(fields[14], out var billCreateEnd) ? billCreateEnd : null,
                            Bill_Creation_Count = ParserFunctions.GetInt(fields[15]),
                            Bill_Creation_Non_Child_Customer_Count = ParserFunctions.GetInt(fields[16]),
                            Bill_Creation_Child_Customer_Count = ParserFunctions.GetInt(fields[17]),
                            Bill_Creation_Per_Minute = ParserFunctions.GetDouble(fields[18]),
                            Print_Batch_Count = ParserFunctions.GetInt(fields[19]),
                            Print_Batch_First_Start_Date = ParserFunctions.GetDateTime(fields[20]),
                            Print_Batch_Last_End_Date = ParserFunctions.GetDateTime(fields[21]),
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

#pragma warning disable CS8601 // Possible null reference assignment.
                        billRunMetric.Statement_Create_Batch_ID = ParserFunctions.GetInt(sqlReader[0]);
                        billRunMetric.Bill_Run_Date = Convert.ToString(sqlReader[1]);
                        billRunMetric.Bill_Run_Completed_Date = Convert.ToString(sqlReader[2]);
                        billRunMetric.Bill_Run_Total_Duration_Minutes = ParserFunctions.GetDouble(sqlReader[3]);
                        billRunMetric.Bill_Creation_Duration_Minutes = ParserFunctions.GetDouble(sqlReader[4]);
                        billRunMetric.Print_Batch_Duration_Minutes = ParserFunctions.GetDouble(sqlReader[5]);
                        billRunMetric.MRC_Duration_Minutes = ParserFunctions.GetDouble(sqlReader[6]);
                        billRunMetric.MRC_Start_Date = DateTime.TryParse(Convert.ToString(sqlReader[7]), out var mrcStart) ? mrcStart : null;
                        billRunMetric.MRC_End_Date = DateTime.TryParse(Convert.ToString(sqlReader[8]), out var mrcEnd) ? mrcEnd : null;
                        billRunMetric.MRC_Customer_Count = ParserFunctions.GetInt(sqlReader[9]);
                        billRunMetric.MRC_Count = ParserFunctions.GetInt(sqlReader[10]);
                        billRunMetric.MRCs_Per_Minute = ParserFunctions.GetDouble(sqlReader[11]);
                        billRunMetric.MRC_To_Bill_Delay_Minutes = ParserFunctions.GetDouble(sqlReader[12]);
                        billRunMetric.Bill_Creation_Start_Date = DateTime.TryParse(Convert.ToString(sqlReader[13]), out var billCreateStart) ? billCreateStart : null;
                        billRunMetric.Bill_Creation_End_Date = DateTime.TryParse(Convert.ToString(sqlReader[14]), out var billCreateEnd) ? billCreateEnd : null;
                        billRunMetric.Bill_Creation_Count = ParserFunctions.GetInt(sqlReader[15]);
                        billRunMetric.Bill_Creation_Non_Child_Customer_Count = ParserFunctions.GetInt(sqlReader[16]);
                        billRunMetric.Bill_Creation_Child_Customer_Count = ParserFunctions.GetInt(sqlReader[17]);
                        billRunMetric.Bill_Creation_Per_Minute = ParserFunctions.GetDouble(sqlReader[18]);
                        billRunMetric.Print_Batch_Count = ParserFunctions.GetInt(sqlReader[19]);
                        billRunMetric.Print_Batch_First_Start_Date = ParserFunctions.GetDateTime(sqlReader[20]);
                        billRunMetric.Print_Batch_Last_End_Date = ParserFunctions.GetDateTime(sqlReader[21]);
#pragma warning restore CS8601 // Possible null reference assignment.
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

            await Task.Run(() =>
            {
                foreach (var metric in metrics)
                {
                    var applicableRestarts = restarts.Where(r => r.AppServer.Equals(CurrrentClientsAppServer) || r.AppServer.Equals(ClientsAppServerShort));

                    if (metric.MRC_Start_Date.HasValue && metric.MRC_End_Date.HasValue)
                    {
                        if (applicableRestarts.Any(r => metric.MRC_Start_Date <= r.MRCRestartTime && r.MRCRestartTime <= metric.MRC_End_Date))
                        {
                            metric.RecurringBillingRestartCount++;
                            mrcRestarts++;
                        }
                    }

                    if (metric.Bill_Creation_Start_Date.HasValue && metric.Bill_Creation_End_Date.HasValue)
                    {
                        if (applicableRestarts.Any(r => metric.Bill_Creation_Start_Date <= r.CreateStatementRestartTime && r.CreateStatementRestartTime <= metric.Bill_Creation_End_Date))
                        {
                            metric.CreateStatementsRestartCount++;
                            csRestarts++;
                        }
                    }
                }

            }, ct);
            AddMessage($"Bill runs impacted by CreateStatements restarts: {csRestarts}.");
            AddMessage($"Bill runs impacted by RecurringBilling restarts: {mrcRestarts}.");

            return metrics;
        }
        private async Task<string> WriteClientCSVFile(List<BillRunMetricsResults> metrics, CancellationToken ct)
        {
            var path = Path.Combine(ClientDirectory, BillRunStatsWithRestartsFileName);
            AddMessage($"Writing new file to {path}");
            try
            {
                using var writer = new StreamWriter(path);

                var header = string.Join(",", BillRunMetricsResults.GetCSVHeader());
                await writer.WriteLineAsync(header);

                await Task.Run(async () =>
                {
                    foreach (var metric in metrics)
                    {
                        string line = metric.GetCSVLine();
                        await writer.WriteLineAsync(line);
                    }
                }, ct);
            }
            catch (Exception ex)
            {
                AddMessage("Trouble writing file: " + ex.Message);
            }
            return path;
        }

        private async Task CreateCombinedExcelFile(IEnumerable<string> csvFilePaths)
        {
            AddMessage("Creating combined metrics file...");

            // Create 1 mega sheet or 1 sheet per client?
            bool doSingleSheet = this.AddAllClientsSheetCheckBox.Checked;
            int currentRow = 1;

            using ExcelPackage package = new();
            ExcelWorksheet worksheet = null;
            if (doSingleSheet)
            {
                worksheet = package.Workbook.Worksheets.Add("ResultsCombined");
            }
            
            foreach (var clientMetrics in ClientBillRunMetrics)
            {
                if (!doSingleSheet)
                {
                    worksheet = package.Workbook.Worksheets.Add(clientMetrics.Key);
                    currentRow = 1;
                }

                if (worksheet == null)
                    throw new NullReferenceException($"worksheet is null somehow! Form1.cs line 351.");

                // Header
                if (currentRow == 1)
                {
                    var header = BillRunMetricsResults.GetCSVHeader(includeClientName: doSingleSheet);
                    for (int i = 0; i < header.Count; i++)
                    {
                        worksheet.Cells[currentRow, i + 1].Value = header[i];
                    }
                    currentRow++;
                }

                // Data
                for (int j = 0; j < clientMetrics.Value.Count; j++)
                {
                    var item = clientMetrics.Value[j];
                    int offset = 0;
                    if (doSingleSheet)
                    {
                        offset = 1;
                        worksheet.Cells[currentRow, offset].Value = clientMetrics.Key;
                    }

                    worksheet.Cells[currentRow, offset + 1].Value = item.Statement_Create_Batch_ID;
                    worksheet.Cells[currentRow, offset + 2].Value = item.Bill_Run_Date;
                    worksheet.Cells[currentRow, offset + 3].Value = item.Bill_Run_Completed_Date;
                    worksheet.Cells[currentRow, offset + 4].Value = item.Bill_Run_Total_Duration_Minutes;
                    worksheet.Cells[currentRow, offset + 5].Value = item.Bill_Creation_Duration_Minutes;
                    worksheet.Cells[currentRow, offset + 6].Value = item.Print_Batch_Duration_Minutes;
                    worksheet.Cells[currentRow, offset + 7].Value = item.MRC_Duration_Minutes;
                    worksheet.Cells[currentRow, offset + 8].Value = item.MRC_Start_Date;
                    worksheet.Cells[currentRow, offset + 9].Value = item.MRC_End_Date;
                    worksheet.Cells[currentRow, offset + 10].Value = item.MRC_Customer_Count;
                    worksheet.Cells[currentRow, offset + 11].Value = item.MRC_Count;
                    worksheet.Cells[currentRow, offset + 12].Value = item.MRCs_Per_Minute;
                    worksheet.Cells[currentRow, offset + 13].Value = item.MRC_To_Bill_Delay_Minutes;
                    worksheet.Cells[currentRow, offset + 14].Value = item.Bill_Creation_Start_Date;
                    worksheet.Cells[currentRow, offset + 15].Value = item.Bill_Creation_End_Date;
                    worksheet.Cells[currentRow, offset + 16].Value = item.Bill_Creation_Count;
                    worksheet.Cells[currentRow, offset + 17].Value = item.Bill_Creation_Non_Child_Customer_Count;
                    worksheet.Cells[currentRow, offset + 18].Value = item.Bill_Creation_Child_Customer_Count;
                    worksheet.Cells[currentRow, offset + 19].Value = item.Bill_Creation_Per_Minute;
                    worksheet.Cells[currentRow, offset + 20].Value = item.Print_Batch_Count;
                    worksheet.Cells[currentRow, offset + 21].Value = item.Print_Batch_First_Start_Date;
                    worksheet.Cells[currentRow, offset + 22].Value = item.Print_Batch_Last_End_Date;
                    worksheet.Cells[currentRow, offset + 23].Value = item.RecurringBillingRestartCount > 0 ? item.RecurringBillingRestartCount : "";
                    worksheet.Cells[currentRow, offset + 24].Value = item.CreateStatementsRestartCount > 0 ? item.CreateStatementsRestartCount : "";

                    currentRow++;
                }
            }

            var xlsxFileName = doSingleSheet ? "ResultsCombined.xlsx" : "CombinedMetrics.xlsx";
            var xlsxFilePath = Path.Combine(FolderPath, xlsxFileName);
            if (File.Exists(xlsxFilePath))
            {
                try
                {
                    File.Delete(xlsxFilePath);
                }
                catch (Exception e)
                {
                    AddMessage($"Error: Could not delete '{xlsxFileName}': " + e.Message);
                }
            }
            package.SaveAs(new FileInfo(xlsxFilePath));
            AddMessage("Combined metrics file created at " + xlsxFilePath);
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            // This usually runs too fast to even click the button, but if it ever gets hung up for whatever reason, this should work. 
            AddMessage("Cancel clicked.");
            CTS.Cancel();
        }

        //private List<string> Messages { get; set; } = new List<string>();
        private void AddMessage(string message)
        {
            var timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            var messageWithTimestamp = $"{timestamp}: {message}";
            Messages.Add(messageWithTimestamp);
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

        private void CheckBoxAll_CheckedChanged(object sender, EventArgs e)
        {
            var checkOrUncheck = CheckBoxAll.Checked;
            for (int i = 0; i < checkedListBox1.Items.Count; i++)
            {
                checkedListBox1.SetItemChecked(i, checkOrUncheck);
            }
        }
    }
}