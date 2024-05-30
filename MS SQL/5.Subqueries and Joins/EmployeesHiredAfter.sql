-- Show all employees that:
-- Are hired after 1/1/1999
-- Are either in "Sales" or "Finance" department

SELECT 
		e.[FirstName],
		e.[LastName],
		e.[HireDate],
		d.[Name] AS [DeptName] 
  FROM    [Employees] AS e
  JOIN    [Departments] as d ON e.DepartmentID = d.DepartmentID
 WHERE    [Name] = 'Sales' OR [Name] = 'Finance' AND [HireDate] > '01-01-1999'
 ORDER BY [HireDate]
