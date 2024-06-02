USE [Softuni]


--When we have aggregation, in this  case COUNT(), we have to group the non-aggregatable vallues, d.[DepartmentID],d.[Name]
SELECT 
	   d.[DepartmentID],
	   d.[Name],
	   e.[Salary],
	   COUNT(d.[DepartmentID]) AS [NumberOfEmployees]
  FROM [Employees] AS e
  JOIN [Departments] AS d ON e.DepartmentID = d.DepartmentID
 GROUP BY d.[DepartmentID],d.[Name],e.[Salary]
 ORDER BY [DepartmentID]