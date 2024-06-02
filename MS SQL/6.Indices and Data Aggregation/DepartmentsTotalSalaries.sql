--Use "SoftUni" database to create a query which prints the total sum of salaries for each department
--Order them by DepartmentID (ascending)

USE [SoftUni]

  SELECT 
		 e.[DepartmentID],
		 SUM(e.[Salary]) AS [TotalSalary]
    FROM [Employees] AS e 
GROUP BY e.[DepartmentID]
ORDER BY [DepartmentID]