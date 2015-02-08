-- Some statistics
USE HabraStats

-- Post count by month
SELECT COUNT(Id), DATEADD(MONTH, DATEDIFF(MONTH, 0, Date), 0) AS Month FROM Posts
GROUP BY DATEADD(MONTH, DATEDIFF(MONTH, 0, Date), 0)
ORDER BY Month