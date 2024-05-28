--1.Find Names of All Employees by First Name
USE [SoftUni]

SELECT [FirstName], [LastName]
FROM [Employees]
WHERE [FirstName] LIKE 'Sa%'

--Second solution
SELECT [FirstName], [LastName]
FROM [Employees]
WHERE LEFT([FirstName],2) = 'Sa'

--Third solution
SELECT [FirstName], [LastName]
FROM [Employees]
WHERE SUBSTRING([FirstName],1,2) = 'Sa'

--2.Find Names of All Employees by Last Name 
SELECT [FirstName], [LastName]
FROM [Employees]
WHERE [LastName] LIKE '%ei%'

--3.Find First Names of All Employees
SELECT [FirstName]
FROM [Employees]
WHERE [DepartmentID] IN(3,10)
AND DATEPART(YEAR,[HireDate]) BETWEEN 1995 AND 2005

--4.Find All Employees Except Engineers
--NOT LIKE is the opposite of  LIKE. In C# isTrue, !isTrue
SELECT [FirstName],[LastName]
FROM [Employees]
WHERE [JobTitle] NOT LIKE '%engineer%'

--5.Find Towns with Name Length
SELECT [Name] 
 FROM [Towns]
WHERE LEN([Name]) = 5 OR LEN([Name]) = 6
ORDER BY [Name]

--6.Find Towns Starting With
SELECT [TownId],[Name] FROM [Towns]
WHERE [Name] LIKE 'M%' 
   OR [Name] LIKE 'K%'
   OR [Name] LIKE 'B%'
   OR [Name] LIKE 'E%'
ORDER BY [Name]

--Second solution
SELECT [TownId],[Name] FROM [Towns]
WHERE [Name] LIKE '[MKBE]%' 
  ORDER BY [Name]

--7.Find Towns Not Starting With
SELECT [TownId],[Name] FROM [Towns]
WHERE [Name] NOT LIKE '[RBD]%'  --   =   '[^RBD]%' 
  ORDER BY [Name]

--8.Create View Employees Hired After 2000 Year

CREATE VIEW V_EmployeesHiredAfter2000 AS
SELECT [FirstName], [LastName]
FROM Employees
WHERE DATEPART(YEAR,[HireDate]) > 2000

--9.Length of Last Name
SELECT [FirstName],[LastName] 
FROM [Employees]
WHERE LEN([LastName]) = 5

--10.Rank Employees by Salary
	 SELECT	   
			[EmployeeID],
			[FirstName],
			[LastName],
			[Salary],
			DENSE_RANK() OVER (PARTITION BY [Salary] ORDER BY [EmployeeID]) AS [Rank]
       FROM [Employees]
	  WHERE [Salary] BETWEEN 10000 AND 50000
   ORDER BY [Salary] DESC

--11.Find All Employees with Rank 2
--Common table expression
--The ORDER BY clause is invalid in views, inline functions, derived tables, subqueries, and common table expressions, unless TOP, OFFSET or FOR XML is also specified.
--We move ORDER BY outside the common language expression
WITH CTE_RankedEmpoyees AS
(
		 SELECT	   
				[EmployeeID],
				[FirstName],
				[LastName],
				[Salary],
				DENSE_RANK() OVER (PARTITION BY [Salary] ORDER BY [EmployeeID]) AS [Rank]
		   FROM [Employees]
		  WHERE [Salary] BETWEEN 10000 AND 50000
	   
)

SELECT * FROM CTE_RankedEmpoyees
WHERE [Rank] = 2
ORDER BY [Salary] DESC


--With Nested querries
SELECT * FROM
(
		SELECT	   
				[EmployeeID],
				[FirstName],
				[LastName],
				[Salary],
				DENSE_RANK() OVER (PARTITION BY [Salary] ORDER BY [EmployeeID]) AS [Rank]
		   FROM [Employees]
		  WHERE [Salary] BETWEEN 10000 AND 50000

) AS [Result]
WHERE [Rank] = 2


--12.Countries Holding 'A' 3 or More Times
-- '%a%a%a%' stands for three times a
USE [Geography]

  SELECT [CountryName] AS [Country Name],[IsoCode] AS [ISO Code]
    FROM [Countries]
   WHERE [CountryName] LIKE '%a%a%a%'
ORDER BY [IsoCode]

--13.Mix of Peak and River Names
SELECT [PeakName],
	   [RiverName],
       LOWER(CONCAT(PeakName,SUBSTRING(RiverName,2,LEN(RiverName)-1))) AS [Mix]
 FROM [Peaks],[Rivers]
WHERE RIGHT([PeakName],1) = LEFT([RiverName],1)
ORDER BY [PeakName],[RiverName]

--14.Games from 2011 and 2012 Year

USE [Diablo]

SELECT TOP 50 
           [Name],
    FORMAT([Start],'yyyy-MM-dd','en-EN') AS [Start]
      FROM [Games]
     WHERE DATEPART(YEAR,[Start]) BETWEEN 2011 AND 2012
  ORDER BY [Start],[Name]

--15.User Email Providers
  SELECT [Username],SUBSTRING([Email],CHARINDEX('@',[Email],1) + 1,LEN([Email])) AS [Email Provider]
    FROM [Users]
ORDER BY [Email Provider],[Username]


--16.Get Users with IP Address Like Pattern
SELECT [Username],[IpAddress] 
  FROM [Users]
 WHERE [IpAddress] LIKE '___.1%.%.___'
ORDER BY [Username]

--17.Show All Games with Duration and Part of the Day

SELECT [Name] AS [Game], 
	   [Part of the Day] = 
			CASE
				WHEN DATEPART(HOUR,[Start])  < 12 THEN 'Morning'
				WHEN DATEPART(HOUR,[Start])  < 18 THEN 'Afternoon'
				ELSE 'Evening'
					END,
        [Duration] = 
			CASE
				WHEN [Duration] <= 3 THEN 'Extra Short'
				WHEN [Duration] <= 6 THEN 'Short'
				WHEN [Duration] > 6 THEN 'Long'
				ELSE 'Extra Long'
					END
FROM [Games]
ORDER BY [Name],[Duration],[Part of the Day]



--18.Orders Table
USE [Orders]

SELECT
	    [ProductName],
	    [OrderDate],
	    DATEADD(DAY,3,[OrderDate]) AS [Pay Due],
	    DATEADD(MONTH,1,[OrderDate]) AS [Delivery Due]
  FROM [Orders]

--19.People Table
USE [MyFirstDb2024]

CREATE TABLE [People]
(
	[Id] INT PRIMARY KEY IDENTITY,
	[Name] VARCHAR(60) NOT NULL,
	[Birthdate] DATETIME2 NOT NULL
)

INSERT INTO [People]
     VALUES ('Victor','2000-12-07 00:00:00.000'),
			('Steven','1992-09-10 00:00:00.000'),
			('Stephen','1910-09-19 00:00:00.000'),
			('John','2010-01-06 00:00:00.000')

SELECT * FROM [People]

INSERT INTO [People]
     VALUES ('SSSD','1991-04-04 00:00:00.000')

SELECT [Name],
	   DATEDIFF(YEAR,[Birthdate],GETDATE()) AS [Age in Years],
	   DATEDIFF(MONTH,[Birthdate],GETDATE()) AS [Age in Months],
	   DATEDIFF(WEEK,[Birthdate],GETDATE()) AS [Age in Weeks],
	   DATEDIFF(DAY,[Birthdate],GETDATE()) AS [Age in Days],
	   DATEDIFF(MINUTE,[Birthdate],GETDATE()) AS [Age in Minutes]

FROM [People]