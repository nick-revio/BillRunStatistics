# BillRunStatistics
Import Bill Run Statistics and Service restarts, export bill run statistics with interruptions noted per batch.

# Possible Improvements
Default the folder paths and file names for reading & writing, but allow the user to override them. 

Create a log file on closing. This probably doesn't provide much value. 

# Stretch Improvements
It would be nicer if we could read the bill run data directly from the database(s). It would probably take me more time than it's worth to figure this out and get it working. 

It would also be nice to grab service restart data right from DataDog but I don't even know where to start with that. 

For now, I'm happy with having the initial data in CSVs somewhere. 