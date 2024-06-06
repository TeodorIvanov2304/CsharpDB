USE [SoftUni]
CREATE PROC usp_SelectEmployeesBySeniority
(@minYearsAtWork int = 5)
AS
 SELECT FirstName, LastName, HireDate,
 DATEDIFF(Year, HireDate, GETDATE()) as Years
 FROM Employees
 WHERE DATEDIFF(Year, HireDate, GETDATE()) > @minYearsAtWork
 ORDER BY HireDate
GO
EXEC usp_SelectEmployeesBySeniority 10