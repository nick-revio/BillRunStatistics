namespace BillRunStatisticsAndRestarts
{
    public  class ServiceRestartInfo
    {
        public ServiceRestartInfo() { }

        public string AppServer { get; set; } = "";
        public DateTime? MRCRestartTime { get; set; } = null;
        public DateTime? CreateStatementRestartTime { get; set; } = null;

    }
}
