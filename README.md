# BillRunStatistics
Import bill run Statistics and service restarts, export bill run statistics with interruptions noted per batch.

## Quick Start
1. Make a directory at C:\Temp\BillRunMetrics\{ClientName} where {ClientName} is one of those listed below.
2. Add a Metrics.csv file with the bill run metrics
3. Add a ServiceRestarts.csv file with the service restart data
4. Run the program, click "Do Stuff"
5. You should now have a MetricsWithRestarts.csv file that contains the bill run statistics plus two additional columns indicating whether or not each bill run required (or was interrupted by) service restarts.

Currently compaitble with 11 clients:
- HUNTER
- NUWAVE
- SKYSWITCH
- CLEARRATE
- OTELSYSTEMS
- CALLTOWER
- SPECTROTEL
- CCI
- MACHNETWORKS
- BBOSOLUTIONS
- TOUCHTONE

## Possible Improvements
- Default the folder paths and file names for reading & writing, but allow the user to override them. 
- Create a log file on closing. This probably doesn't provide much value. 

## Stretch Improvements
- Have the app read the bill run data directly from the database(s). This would probably take me more time than it's worth to figure it out & get it working.  
- Have the app get the service restart data directly from DataDog. I don't even know where to start with that. 
For now, I'm happy with having the initial data in CSVs somewhere. 
