using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BillRunStatisticsAndRestarts
{
    public class BillRunMetricsResults
    {
        public BillRunMetricsResults() { }

        public string Statement_Create_Batch_ID { get; set; }
        public string Bill_Run_Date { get; set; }
        public string Bill_Run_Completed_Date { get; set; }
        public string Bill_Run_Total_Duration_Minutes { get; set; }
        public string Bill_Creation_Duration_Minutes { get; set; }
        public string Print_Batch_Duration_Minutes { get; set; }
        public string MRC_Duration_Minutes { get; set; }
        public DateTime? MRC_Start_Date { get; set; }
        public DateTime? MRC_End_Date { get; set; }
        public string MRC_Customer_Count { get; set; }
        public string MRC_Count { get; set; }
        public string MRCs_Per_Minute { get; set; }
        public string MRC_To_Bill_Delay_Minutes { get; set; }
        public DateTime? Bill_Creation_Start_Date { get; set; }
        public DateTime? Bill_Creation_End_Date { get; set; }
        public string Bill_Creation_Count { get; set; }
        public string Bill_Creation_Non_Child_Customer_Count { get; set; }
        public string Bill_Creation_Child_Customer_Count { get; set; }
        public string Bill_Creation_Per_Minute { get; set; }
        public string Print_Batch_Count { get; set; }
        public string Print_Batch_First_Start_Date { get; set; }
        public string Print_Batch_Last_End_Date { get; set; }
        public string RecurringBillingRestarted { get; set; }
        public string CreateStatementsRestarted { get; set; }

        public string GetCSVLine()
        {
            Type type = typeof(BillRunMetricsResults);
            PropertyInfo[] properties = type.GetProperties();

            StringBuilder sb = new StringBuilder();

            foreach (PropertyInfo property in properties)
            {
                string propertyName = property.Name;
                object propertyValue = property.GetValue(this);
                sb.Append(propertyValue);
                sb.Append(",");
            }
            return sb.ToString();
        }

    }
}
