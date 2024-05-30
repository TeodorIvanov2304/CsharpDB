USE [tempdb]

CREATE TABLE #Employees
(
	[Id] INT PRIMARY KEY,
	[FirstName] VARCHAR(50),
	[LastName] VARCHAR(50),
	[Address] VARCHAR(50)
)

SELECT * FROM #Employees