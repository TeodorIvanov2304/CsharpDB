USE [SoftUni]

    SELECT MIN(dt.AvgSalary) AS MinAvgSalary
    FROM 
(
	SELECT AVG(Salary) AS AvgSalary
	FROM Employees
	GROUP BY DepartmentID
)   
	AS dt