using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillRunStatisticsAndRestarts
{
    public  class DataDogData
    {
        public DataDogData() { }

        public string AppServer { get; set; } = "";
        public DateTime? MRCRestartTime { get; set; } = null;
        public DateTime? CreateStatementRestartTime { get; set; } = null;

    }
}
