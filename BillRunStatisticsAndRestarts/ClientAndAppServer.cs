using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillRunStatisticsAndRestarts
{
    public class ClientAndAppServer
    {
        public string ClientName { get; set; }
        public string AppServerName { get; set; }

        public ClientAndAppServer(string client, string appServer)
        {
            ClientName = client;
            AppServerName = appServer;
        }
    }
}
