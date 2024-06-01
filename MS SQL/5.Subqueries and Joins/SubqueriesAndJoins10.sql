USE [SoftUni]

--1.Employee Address
SELECT TOP 5
	   e.EmployeeID,
	   e.JobTitle,
	   a.AddressID,
	   a.AddressText
 FROM [Employees] AS e
 JOIN [Addresses] AS a ON e.AddressID = a.AddressID
ORDER BY [AddressID]

--2.Addresses with Towns

SELECT TOP 50
       e.FirstName,
	   e.LastName,
	   t.[Name] AS [Town],
	   a.AddressText
	   
  FROM [Employees] AS e
  JOIN [Addresses] AS a ON e.AddressID = a.AddressID
  JOIN [Towns]     AS t ON a.TownID = t.TownID
  ORDER BY [FirstName],[LastName]

--3.Sales Employee

SELECT 
	   e.EmployeeID,
       e.FirstName,
	   e.LastName,
	   d.[Name] AS [DepartmentName]
	   
  FROM [Employees] AS e
  JOIN [Departments] AS d ON e.DepartmentID = d.DepartmentID
 WHERE d.[Name] = 'Sales'
 ORDER BY [EmployeeID]

 --4.Employee Departments

 SELECT TOP 5
	   e.EmployeeID,
       e.FirstName,
	   e.Salary,
	   d.[Name] AS [DepartmentName]
	   
  FROM [Employees] AS e
  JOIN [Departments] AS d ON e.DepartmentID = d.DepartmentID
 WHERE [Salary] > 15000
 ORDER BY d.DepartmentID

--5.Employees Without Project
--DISTINCT is for unique matches, and NOT IN is the equivalent of ! in C#

SELECT TOP 3
	   [EmployeeID],
	   [FirstName]
  FROM [Employees]
 WHERE [EmployeeID] NOT IN (SELECT DISTINCT [EmployeeID] FROM [EmployeesProjects])
 ORDER BY [EmployeeID]

-- With IS NULL
SELECT TOP 3
		e.[EmployeeID],
		e.[FirstName]
  FROM [Employees] AS e
  LEFT JOIN [EmployeesProjects] AS ep ON e.[EmployeeID] = ep.[EmployeeID]
 WHERE ep.[EmployeeID] IS NULL


--6.Employees Hired After

SELECT 
       e.[FirstName],
       e.[LastName],
       e.[HireDate],
       d.[Name] AS [DeptName]
  FROM [Employees] AS e
  JOIN [Departments] AS d ON e.DepartmentID = d.DepartmentID
 WHERE [HireDate] > '01-01-1999' AND d.[Name] IN ('Finance','Sales')
 ORDER BY [HireDate]

--7.Employees with Project

SELECT TOP 5
	   e.EmployeeID,
	   e.FirstName,
	   p.[Name] AS [ProjectName]
  FROM  [Employees] AS e
  JOIN  [EmployeesProjects] AS ep ON e.EmployeeID = ep.EmployeeID
  JOIN  [Projects] AS p ON ep.ProjectID = p.ProjectID
 WHERE [StartDate] > '08-13-2002' AND [EndDate] IS NULL
 ORDER BY [EmployeeID]


--8.Employee 24

SELECT 
	   e.[EmployeeID],
	   e.[FirstName],
  CASE 
	   WHEN p.[StartDate] > '12-31-2004' THEN NULL
	   ELSE p.[Name]
   END AS [ProjectName]
  FROM [Employees] AS e
  JOIN [EmployeesProjects] AS ep ON e.EmployeeID = ep.EmployeeID
  JOIN [Projects] AS p ON ep.[ProjectID] = p.[ProjectID]
 WHERE e.[EmployeeID] = 24  

 --9.Employee Manager

--With RIGHT JOIN

SELECT 
	   e.[EmployeeID],
	   e.[FirstName],
	   e.[ManagerID],
	   re.[FirstName] AS [ManagerName]
  FROM [Employees] AS e
  JOIN [Employees] as re ON e.ManagerID = re.EmployeeID
 WHERE e.[ManagerID] IN (3,7)
 ORDER BY [EmployeeID]

--With LEFT JOIN
 SELECT 
	   re.EmployeeID,
	   re.FirstName,
	   re.ManagerID,
	   e.FirstName AS [ManagerName]
  FROM [Employees] AS e
  JOIN [Employees] as re ON e.EmployeeID = re.ManagerID
 WHERE re.[ManagerID] IN (3,7)
 ORDER BY [EmployeeID]

 --10.Employees Summary
 SELECT TOP 50
	   re.EmployeeID,
	   CONCAT_WS(' ',re.FirstName,re.LastName) AS [EmployeeName],
	   CONCAT_WS(' ',e.FirstName,e.LastName) AS [ManagerName],
	   d.[Name] AS [DepartmentName]
  FROM [Employees] AS e
  JOIN [Employees] AS re ON e.EmployeeID = re.ManagerID
  JOIN [Departments] AS d ON re.DepartmentID = d.DepartmentID
 ORDER BY [EmployeeID]


 SELECT * FROM [Employees]

--11.Min Average Salary
SELECT MIN(dt.[AvgSalary]) AS [MinAvgSalary]
  FROM
  (  
	  SELECT AVG(Salary) AS [AvgSalary]
		FROM [Employees]
	GROUP BY [DepartmentID]
  )

    AS dt
--Without subquery
	  SELECT TOP 1 AVG(Salary) AS [AvgSalary]
		FROM [Employees]
	GROUP BY [DepartmentID]
	ORDER BY [AvgSalary]


--12.Highest Peaks in Bulgaria
USE [Geography]

SELECT 
        c.CountryCode  AS [Country Code],
	    m.MountainRange,
	    p.PeakName,
	    p.Elevation
  FROM  [Countries] AS c
  JOIN  [MountainsCountries] AS mc ON c.CountryCode = mc.CountryCode
  JOIN  [Mountains] AS m ON m.Id = mc.MountainId
  JOIN  [Peaks] AS p ON p.MountainId = m.Id
 WHERE  c.[CountryCode] = 'BG' AND p.Elevation > 2835
 ORDER  BY [Elevation] DESC

--13.Count Mountain Ranges
 
 SELECT 
       c.CountryCode,
	   COUNT(*) AS [MountainRanges]
  FROM [Countries] AS c
  JOIN [MountainsCountries] AS mc ON c.CountryCode = mc.CountryCode
  JOIN [Mountains] AS m ON mc.MountainId = m.Id
  WHERE c.[CountryCode] IN ('BG','RU','US')
  GROUP BY c.[CountryCode]

--14.Countries With or Without Rivers
   SELECT TOP 5
		  c.[CountryName],
		  r.[RiverName]
	 FROM [Countries] AS c
LEFT JOIN [CountriesRivers] AS cr ON c.CountryCode = cr.CountryCode
LEFT JOIN [Rivers] AS r ON cr.RiverId = r.Id
    WHERE [ContinentCode] = 'AF'
 ORDER By [CountryName]


--16.Countries Without Any Mountains
 
 SELECT COUNT(*) 
  FROM [Countries]
 WHERE [CountryCode] NOT IN (SELECT DISTINCT [CountryCode] FROM [MountainsCountries])

 --17.Highest Peak and Longest River by Country
 --Когато има агрегатна функция, винаги се пише GROUP BY

SELECT TOP (5)
	[CountryName],
	MAX([Elevation]) AS [HighestPeakElevation],
	MAX(r.[Length]) AS [LongestRiverLength]
FROM Countries AS c
LEFT JOIN [CountriesRivers] AS cr ON cr.[CountryCode] = c.[CountryCode]
LEFT JOIN [Rivers] AS r ON r.[Id] = cr.[RiverId]
LEFT JOIN [MountainsCountries] AS mc ON mc.[CountryCode] = c.[CountryCode]
LEFT JOIN [Mountains] AS m ON m.[Id] = mc.[MountainId]
LEFT JOIN [Peaks] AS p ON p.[MountainId] = m.[Id]
 GROUP BY 
	      [CountryName]
 ORDER BY
		  [HighestPeakElevation] DESC,
		  [LongestRiverLength] DESC,
		  [CountryName]


--18.Highest Peak Name and Elevation by Country

  WITH PeaksRankedByElevation AS 
(
	SELECT
		c.CountryName,
		p.PeakName,
		p.Elevation,
		m.MountainRange,
		DENSE_RANK() OVER
			(PARTITION BY c.CountryName ORDER BY Elevation DESC) AS PeakRank
	FROM Countries AS c
	LEFT JOIN MountainsCountries AS mc ON c.CountryCode = mc.CountryCode
	LEFT JOIN Mountains AS m ON m.Id = mc.MountainId
	LEFT JOIN Peaks AS p ON m.Id = p.MountainId
)

SELECT TOP(5)
	CountryName AS Country,
	ISNULL(PeakName, '(no highest peak)') AS [Highest Peak Name],
	ISNULL(Elevation, 0) AS [Highest Peak Elevation],
	ISNULL(MountainRange, '(no mountain)') AS Mountain
FROM PeaksRankedByElevation
WHERE PeakRank = 1
ORDER BY 
	CountryName, [Highest Peak Name]
