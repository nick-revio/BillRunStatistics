using System.Reflection;
using System.Text;

namespace BillRunStatisticsAndRestarts
{
    public class BillRunMetricsResults
    {
        public int Statement_Create_Batch_ID { get; set; }
        public string Bill_Run_Date { get; set; } = "";
        public string Bill_Run_Completed_Date { get; set; } = "";
        public double Bill_Run_Total_Duration_Minutes { get; set; }
        public double Bill_Creation_Duration_Minutes { get; set; }
        public double Print_Batch_Duration_Minutes { get; set; }
        public double MRC_Duration_Minutes { get; set; }
        public DateTime? MRC_Start_Date { get; set; }
        public DateTime? MRC_End_Date { get; set; }
        public int MRC_Customer_Count { get; set; }
        public int MRC_Count { get; set; }
        public double MRCs_Per_Minute { get; set; }
        public double MRC_To_Bill_Delay_Minutes { get; set; }
        public DateTime? Bill_Creation_Start_Date { get; set; }
        public DateTime? Bill_Creation_End_Date { get; set; }
        public int Bill_Creation_Count { get; set; }
        public int Bill_Creation_Non_Child_Customer_Count { get; set; }
        public int Bill_Creation_Child_Customer_Count { get; set; }
        public double Bill_Creation_Per_Minute { get; set; }
        public int Print_Batch_Count { get; set; }
        public DateTime? Print_Batch_First_Start_Date { get; set; }
        public DateTime? Print_Batch_Last_End_Date { get; set; }
        public int RecurringBillingRestartCount { get; set; }
        public int CreateStatementsRestartCount { get; set; }

        public string GetCSVLine()
        {
            Type type = typeof(BillRunMetricsResults);
            PropertyInfo[] properties = type.GetProperties();

            StringBuilder sb = new();

            foreach (PropertyInfo property in properties)
            {
                var propertyName = property.Name;
                object? propertyValue = property.GetValue(this);

                if (BlankIfZeroFields.Contains(propertyName) && int.TryParse(Convert.ToString(propertyValue), out int x) && x == 0)
                    propertyValue = "";

                if (DateOnlyFields.Contains(propertyName) && DateTime.TryParse(Convert.ToString(propertyValue), out var dt))
                    propertyValue = dt.ToString("yyyy-MM-dd");

                sb.Append(propertyValue);
                sb.Append(",");
            }
            return sb.ToString();
        }

        protected HashSet<string> BlankIfZeroFields = new HashSet<string>()
        { 
            nameof(RecurringBillingRestartCount), 
            nameof(CreateStatementsRestartCount)
        };
        protected HashSet<string> DateOnlyFields = new HashSet<string>()
        {
            nameof(Bill_Run_Date)
        };

        public static List<string> GetCSVHeader(bool includeClientName = false)
        {
            Type type = typeof(BillRunMetricsResults);
            PropertyInfo[] properties = type.GetProperties();

            var headers = new List<string>();

            if (includeClientName)
            {
                headers.Add("ClientName");
            }

            foreach (PropertyInfo property in properties)
            {
                var propertyName = property.Name;
                if (propertyName == nameof(Bill_Creation_Duration_Minutes))
                {
                    propertyName += " (CreateStatements)";
                }
                if (propertyName == nameof(MRC_Duration_Minutes))
                {
                    propertyName += " (RecurringBilling)";
                }
                headers.Add(propertyName);
            }
            return headers;
        }

    }
}
