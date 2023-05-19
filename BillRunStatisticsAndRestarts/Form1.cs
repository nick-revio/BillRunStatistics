using System.Diagnostics.Metrics;
using System.IO;
using System.Linq;
using System.Security.Policy;

namespace BillRunStatisticsAndRestarts
{
    public partial class Form1 : Form
    {


        public readonly string FolderPath = "C:\\Temp\\BillRunMetrics";
        public readonly string ServiceRestartsDataFile = "ServiceRestarts.csv";
        public readonly string BillRunStatsFile = "Metrics.csv";
        public readonly string BillRunStatsWithRestartsFileName = "MetricsWithRestarts.csv";

        public string ClientDirectory { get; set; } = "";
        public string ClientsAppServer { get; set; } = "";

        public List<ClientAndAppServer> ClientAndAppServers = new()
        {
            new ClientAndAppServer("HUNTER","app11.core00.rev.io"),
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

        public List<string> Fields = new()
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

        public List<string> AddFields = new()
        {
            "RecurringBillingRestarted",
            "CreateStatementsRestarted"
        };

        public CancellationTokenSource cts { get; set; }

        public Form1()
        {
            InitializeComponent();
            ClientDirectory = "";
            cts = new CancellationTokenSource();
            AddMessage("Ready to run!");
        }

        private async void Button1_Click(object sender, EventArgs e)
        {
            cts = new CancellationTokenSource();
            ClientDirectory = Path.Combine(FolderPath, clientTextBox.Text);
            AddMessage($"Client Directory set to {ClientDirectory}");

            var path = Path.Combine(ClientDirectory, BillRunStatsFile);
            AddMessage($"Reading bill run data from {path}");

            SetClientsAppServer();

            var metrics = await ReadOriginalMetrics(path, cts.Token);

            var serviceRestarts = await ReadServiceRestartData(cts.Token);

            var updatedMetrics = await DetermineBillRunsImpactedByServiceRestarts(metrics, serviceRestarts, cts.Token);
            await WriteFile(updatedMetrics, cts.Token);
            AddMessage("Complete.");
        }

        private async Task<List<BillRunMetricsResults>> ReadOriginalMetrics(string path, CancellationToken ct)
        {
            AddMessage("Reading Bill Run Metrics...");
            var results = new List<BillRunMetricsResults>();

            using (var reader = new StreamReader(path))
            {
                string? line = await reader.ReadLineAsync();

                while (line != null && !ct.IsCancellationRequested)
                {
                    if (ct.IsCancellationRequested) break;
                    string[] fields = line.Split(",");

                    if (fields.Length >= 9)
                    {
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
                    }
                    line = await reader.ReadLineAsync();
                }
            }

            return results;
        }

        private async Task<List<ServiceRestartInfo>> ReadServiceRestartData(CancellationToken ct)
        {
            AddMessage("Reading service restart data...");
            var dataDogs = new List<ServiceRestartInfo>();
            var path = Path.Combine(ClientDirectory, ServiceRestartsDataFile);

            using (var reader = new StreamReader(path))
            {
                string? line = await reader.ReadLineAsync();

                while (line != null && !ct.IsCancellationRequested)
                {
                    string[] fields = line.Split(",");

                    if (fields.Length >= 3)
                    {
                        var result = new ServiceRestartInfo
                        {
                            AppServer = fields[0].ToUpper(),
                            MRCRestartTime = DateTime.TryParse(fields[1], out var mrcRestartTime) ? mrcRestartTime : null,
                            CreateStatementRestartTime = DateTime.TryParse(fields[2], out var createStatementsRestartTime) ? createStatementsRestartTime : null,
                        };

                        dataDogs.Add(result);
                    }
                    line = await reader.ReadLineAsync();
                }
            }

            return dataDogs;
        }
        public async Task<List<BillRunMetricsResults>> DetermineBillRunsImpactedByServiceRestarts(List<BillRunMetricsResults> metrics, List<ServiceRestartInfo> restarts, CancellationToken ct)
        {
            AddMessage("Determining which bill runs required or were interrupted by service restarts...");
            foreach (var metric in metrics)
            {
                var applicableRestarts = restarts.Where(r => r.AppServer.Equals(ClientsAppServer));

                if (metric.MRC_Start_Date.HasValue && metric.MRC_End_Date.HasValue)
                {
                    if (applicableRestarts.Any(r => metric.MRC_Start_Date <= r.MRCRestartTime && r.MRCRestartTime <= metric.MRC_End_Date))
                    {
                        metric.RecurringBillingRestarted = "TRUE";
                    }
                }

                if (metric.Bill_Creation_Start_Date.HasValue && metric.Bill_Creation_End_Date.HasValue)
                {
                    if (applicableRestarts.Any(r => metric.Bill_Creation_Start_Date <= r.CreateStatementRestartTime && r.CreateStatementRestartTime <= metric.Bill_Creation_End_Date))
                    {
                        metric.CreateStatementsRestarted = "TRUE";
                    }
                }

                if (ct.IsCancellationRequested) break;
            }

            return metrics;
        }
        private async Task WriteFile(List<BillRunMetricsResults> metrics, CancellationToken ct)
        {
            var path = Path.Combine(ClientDirectory, BillRunStatsWithRestartsFileName);
            AddMessage($"Writing new file to {path}");
            using (var writer = new StreamWriter(path))
            {
                foreach (var metric in metrics)
                {
                    string line = metric.GetCSVLine();
                    await writer.WriteLineAsync(line);

                    if (ct.IsCancellationRequested) break;
                }
            }
        }

        private void SetClientsAppServer()
        {
            var client = clientTextBox.Text.ToUpper();
            var clientAndAppServer = ClientAndAppServers.Where(x => x.ClientName.ToUpper().Equals(client, StringComparison.OrdinalIgnoreCase)).SingleOrDefault();
            if (clientAndAppServer == null)
                throw new Exception("Could not find client");

            ClientsAppServer = clientAndAppServer.AppServerName.ToUpper();
            AddMessage($"{client}'s services run on {ClientsAppServer}");
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
    }
}