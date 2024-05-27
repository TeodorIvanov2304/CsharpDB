--Cast and Convert
--The two functions are working similary
SELECT CAST(1 AS DECIMAL(10,2))

SELECT CONVERT(DECIMAL(10,2),1)

--Is null
USE [SoftUni]
SELECT [FirstName],
	   ISNULL([MiddleName],'N/A'),
	   [LastName]
	     FROM [Employees]

--Coalesce
SELECT COALESCE(NULL,NULL,'third value','fourth value')

--Offset & fetch

  SELECT [EmployeeID],[FirstName],[LastName]
    FROM [Employees]
ORDER BY [EmployeeID]
   OFFSET 0 ROWS
    FETCH NEXT 100 ROWS ONLY

--Ofset by pages (10)
SELECT * FROM Employees
ORDER BY [EmployeeID]
OFFSET (10 * (10-1)) ROWS
FETCH NEXT 10 ROWS ONLY

--Row number
SELECT TOP 20
	  ROW_NUMBER() OVER (ORDER BY [DepartmentID]) AS "Row Number",
           [FirstName],
		   [LastName],
		   [DepartmentID]
      FROM [Employees]

--Rank

SELECT TOP 20
	  ROW_NUMBER() OVER (ORDER BY [DepartmentID]) AS "Row Number",
           [FirstName],
		   [LastName],
		   [DepartmentID],
		   RANK() OVER (ORDER BY DepartmentID) AS "Rank"
      FROM [Employees]

--Dense rank
SELECT TOP 20
	  ROW_NUMBER() OVER (ORDER BY [DepartmentID]) AS "Row Number",
           [FirstName],
		   [LastName],
		   [DepartmentID],
		   DENSE_RANK() OVER (ORDER BY DepartmentID) AS "Dense Rank"
      FROM [Employees]

--NTILE
SELECT 
	  ROW_NUMBER() OVER (ORDER BY [DepartmentID]) AS "Row Number",
           [FirstName],
		   [LastName],
		   [DepartmentID],
		   NTILE(4) OVER (ORDER BY DepartmentID) AS "Groups"
      FROM [Employees]
	 WHERE [DepartmentID] < 4
