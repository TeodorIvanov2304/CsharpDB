--WHERE filters the resulst set, HAVING filters the groups

USE [SoftUni]

  SELECT 
	     e.[DepartmentID],
	     MIN(e.[Salary]) AS [MinSalary]
    FROM [Employees] AS e
   WHERE e.[DepartmentID] < 15
GROUP BY e.[DepartmentID]
  HAVING MIN(e.[Salary]) < 10000