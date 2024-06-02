--MIN 
 
  SELECT 
	     e.[DepartmentID],
	     MIN(e.[Salary]) AS [MinSalary]
    FROM [Employees] AS e
GROUP BY e.[DepartmentID]


--MAX
  SELECT 
	     e.[DepartmentID],
	     MAX(e.[Salary]) AS [MaxSalary]
    FROM [Employees] AS e
GROUP BY e.[DepartmentID]

--AVG
  SELECT 
	     e.[DepartmentID],
	     AVG(e.[Salary]) AS [AvearageSalary]
    FROM [Employees] AS e
GROUP BY e.[DepartmentID]

--COUNT
  SELECT 
	     e.[DepartmentID],
	     COUNT(e.[Salary]) AS [SalaryCount]
    FROM [Employees] AS e
GROUP BY e.[DepartmentID]


--SUM
  SELECT 
	     e.[DepartmentID],
	     SUM(e.[Salary]) AS [TotalSalary]
    FROM [Employees] AS e
GROUP BY e.[DepartmentID]

--STRING_AGG

   SELECT 
	        d.[Name],
	        e.[Salary],
			          --(Expresion,                           Separator)  
	        STRING_AGG(CONCAT_WS(' ',e.[FirstName],e.[LastName]),', ') 
			--ORDER BY Name
			WITHIN GROUP (ORDER BY CONCAT_WS(' ',e.[FirstName],e.[LastName]))
			AS [Employees]
			
     FROM   [Employees] AS e
     JOIN   [Departments] AS d ON e.DepartmentID = d.DepartmentID
GROUP BY    d.[DepartmentID],d.[Name], e.[Salary]
ORDER BY    d.[DepartmentID]
