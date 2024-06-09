USE SoftUni
GO
CREATE PROC dbo.usp_SelectEmployeesBySeniority
AS
 SELECT *
 FROM Employees
 WHERE DATEDIFF(Year, HireDate, GETDATE()) > 20
GO

EXEC dbo.usp_SelectEmployeesBySeniority