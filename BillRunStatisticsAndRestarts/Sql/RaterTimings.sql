SELECT rl.Rater_Log_ID
      ,rl.Message
      ,rl.Created_Date
      ,Row_Num = ROW_NUMBER() OVER (ORDER BY rl.Rater_Log_ID)
INTO #myTempData
FROM dbo.tblRaterLog rl
WHERE rl.Created_Date > '1/1/2023'
AND   rl.Message IN ( 'Connected to Redis successfully.', REPLACE( DB_NAME(), 'CLIENT_', '') + ' - CDRs found: 50000' )
ORDER BY rl.Rater_Log_ID;

SELECT Time_Take_In_Microsecond = DATEDIFF(MICROSECOND, mtd2.Created_Date, mtd.Created_Date)
      ,Start_Time = mtd2.Created_Date
      ,End_Time = mtd.Created_Date
INTO #myTempTimingData
FROM #myTempData mtd
JOIN #myTempData mtd2 ON mtd2.Row_Num = (mtd.Row_Num - 1)
JOIN #myTempData mtd3 ON mtd3.Row_Num = (mtd.Row_Num - 2)
WHERE mtd.Message = (REPLACE( DB_NAME(), 'CLIENT_', '') + ' - CDRs found: 50000')
AND   mtd2.Message = 'Connected to Redis successfully.'
AND   mtd3.Message = (REPLACE( DB_NAME(), 'CLIENT_', '') + ' - CDRs found: 50000')
ORDER BY mtd.Rater_Log_ID;

SELECT Date_Only = CAST(mttd.Start_Time AS DATE)
      ,Time_Elapsed_AVG = AVG(mttd.Time_Take_In_Microsecond)
FROM #myTempTimingData mttd
GROUP BY CAST(mttd.Start_Time AS DATE)
ORDER BY 1;

DROP TABLE #myTempData, #myTempTimingData;