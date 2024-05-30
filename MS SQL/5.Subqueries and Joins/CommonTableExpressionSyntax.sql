USE [SoftUni]

WITH Employee_CTE (FirstName, LastName, DepartmentName) AS
(

        SELECT 
				e.[FirstName],
				e.[LastName],
				d.[Name]
		  FROM [Employees] AS e
		  LEFT JOIN [Departments] AS d ON d.DepartmentID = e.DepartmentID
)

SELECT * FROM Employee_CTE