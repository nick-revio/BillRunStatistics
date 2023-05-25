using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillRunStatisticsAndRestarts
{
	public class DataDogEvents
	{
		public DataDogEvent[] Events { get; set; }

		public DataDogEvents()
		{
			Events = new DataDogEvent[] { };
		}
	}

	public class DataDogEvent
	{
		public long id { get; set; }
		public string id_str { get; set; }
		public string title { get; set; }
		public string text { get; set; }
		public ddpriority priority { get; set; } // this could be an enum, normal or low
		public long date_happened { get; set; } //unix time
		public string source { get; set; }
		public string alert_type { get; set; } // this could also be an enum probably
		public bool is_aggregate { get; set; }
		public string[] tags { get; set; }
		public string host { get; set; }
		public string device_name { get; set; }
		public string[] comments { get; set; }
		public string url { get; set; }
		public string resource { get; set; }
		public int monitor_id { get; set; }
		public string[] monitor_groups { get; set; }
		public int monitor_group_status { get; set; }

		public DataDogEvent()
		{
			id = 0;
			id_str = string.Empty;
			title = string.Empty;
			text = string.Empty;
			priority = ddpriority.normal;
			date_happened = 0;
			source = string.Empty;
			alert_type = string.Empty;
			tags = new string[] { };
			host = string.Empty;
			device_name = string.Empty;
			comments = new string[] { };
			url = string.Empty;
			resource = string.Empty;
			monitor_id = 0;
			monitor_groups = new string[] { };
			monitor_group_status = 0;
		}

		public enum ddpriority
		{
			low = 0,
			normal = 1
		}
	}
}
