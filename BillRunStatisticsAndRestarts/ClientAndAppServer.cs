namespace BillRunStatisticsAndRestarts
{
    public class ClientAndAppServer
    {
        public string ClientName { get; set; }
        public string AppServerName { get; set; }
        public string AppServerNameShort => StringFunctions.Left(AppServerName, 5).ToUpper();

        public ClientAndAppServer(string client, string appServer)
        {
            ClientName = client;
            AppServerName = appServer;
        }
    }
}
