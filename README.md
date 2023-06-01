<!-- @format -->

# BillRunStatistics

Import bill run Statistics and service restarts, export bill run statistics with interruptions noted per batch.

## Quick Start

1. Make a directory at C:\Temp\BillRunMetrics\{ClientName} where {ClientName} is one of those listed below.
2. Add a ServiceRestarts.csv file with the service restart data. You can get this data by entering the datadogs keys in the batch file and running it.
3. Run the program, click "Get Bill Run Metrics"
4. You should now have a MetricsWithRestarts.csv file that contains the bill run statistics plus two additional columns indicating whether or not each bill run required (or was interrupted by) service restarts.
5. If the "Create Combined File" box was checked, there should be a CombinedMetrics.xlsx file with all the metrics.

Currently compaitble with 11 clients:

-   HUNTER
-   NUWAVE
-   SKYSWITCH
-   CLEARRATE
-   OTELSYSTEMS
-   CALLTOWER
-   SPECTROTEL
-   CCI
-   MACHNETWORKS
-   BBOSOLUTIONS
-   TOUCHTONE

## Possible Improvements

-   Default the folder paths and file names for reading & writing, but allow the user to override them.
-   Create a log file on exceptions. Closing the program dumps a log file, but exceptions bypass this.
-   Give the app the ability to grab the datadog data itself. The only blocker would be how to secure the keys.
