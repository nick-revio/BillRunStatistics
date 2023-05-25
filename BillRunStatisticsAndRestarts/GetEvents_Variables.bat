SET tags=windows_service:oc.services.h2o.recurringbilling
SET api-key="DD-API-KEY: REDACTED"
SET app-key="DD-APPLICATION-KEY: REDACTED"

REM January
SET startposix=1672549200
SET endposix=1675227600
SET outputfile=C:\Temp\BillRunMetrics\ServiceRestarts\January.json
curl --get "https://api.us3.datadoghq.com/api/v1/events?start=%startposix%&end=%endposix%&tags=%tags%&exclude_aggregate=true" --header %api-key% --header %app-key% -o %outputfile% | jq
pause 

rem februray
set startposix=1675227600
set endposix=1677646800
SET outputfile=C:\Temp\BillRunMetrics\ServiceRestarts\February.json

curl --get "https://api.us3.datadoghq.com/api/v1/events?start=%startposix%&end=%endposix%&tags=%tags%&exclude_aggregate=true" --header %api-key% --header %app-key% -o %outputfile% | jq

rem march
set startposix=1677646800
set endposix=1680321600
SET outputfile=C:\Temp\BillRunMetrics\ServiceRestarts\March.json

curl --get "https://api.us3.datadoghq.com/api/v1/events?start=%startposix%&end=%endposix%&tags=%tags%&exclude_aggregate=true" --header %api-key% --header %app-key% -o %outputfile% | jq

rem april
set startposix=1680321600
set endposix=1682913600
SET outputfile=C:\Temp\BillRunMetrics\ServiceRestarts\April.json

curl --get "https://api.us3.datadoghq.com/api/v1/events?start=%startposix%&end=%endposix%&tags=%tags%&exclude_aggregate=true" --header %api-key% --header %app-key% -o %outputfile% | jq

rem may
set startposix=1682913600
set endposix=1685592000
SET outputfile=C:\Temp\BillRunMetrics\ServiceRestarts\May.json

curl --get "https://api.us3.datadoghq.com/api/v1/events?start=%startposix%&end=%endposix%&tags=%tags%&exclude_aggregate=true" --header %api-key% --header %app-key% -o %outputfile% | jq
