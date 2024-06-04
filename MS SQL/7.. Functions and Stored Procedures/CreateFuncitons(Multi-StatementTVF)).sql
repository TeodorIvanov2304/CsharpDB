USE [SoftUni]

CREATE FUNCTION udf_EmployeeListByDepartment(@DepName NVARCHAR(20))
RETURNS @result TABLE(
 [FirstName] NVARCHAR(50) NOT NULL,
 [LastName] NVARCHAR(50) NOT NULL,
 [DepartmentName] NVARCHAR(20) NOT NULL) AS
BEGIN
 WITH Employees_CTE ([FirstName], [LastName], [DepartmentName])
 AS(
 SELECT 
 e.[FirstName], 
 e.[LastName], 
 d.[Name]
 FROM Employees AS e
 LEFT JOIN Departments AS d ON d.DepartmentID = e.DepartmentID)
 INSERT INTO @result 
 SELECT 
 [FirstName], 
 [LastName], 
 [DepartmentName]
 FROM Employees_CTE WHERE DepartmentName = @DepName
 RETURN
END