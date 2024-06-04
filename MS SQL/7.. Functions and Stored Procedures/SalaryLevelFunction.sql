-- Write a function ufn_GetSalaryLevel(@Salary MONEY) that receives salary of an employee and returns the level of the salary
-- If salary is < 30000 return "Low"
-- If salary is between 30000 and 50000 (inclusive) returns "Average"
-- If salary is > 50000 return "High"

CREATE FUNCTION udf_GetSalaryValue(@Salary MONEY)
RETURNS NVARCHAR(10)
AS
BEGIN
	 DECLARE @result NVARCHAR(10)

	 IF(@Salary <30000)
	 BEGIN
		SET @result = 'Low'
	 END

	 ELSE IF(@Salary BETWEEN 30000 AND 50000)
	 BEGIN 
		SET @result = 'Average'
	 END

	 ELSE
	 BEGIN
		SET @result = 'High'
	 END

	 RETURN @result

END

--Execute Funciton

SELECT
	 FirstName,
	 LastName,
	 Salary,
	 dbo.udf_GetSalaryValue(Salary) AS [SalaryLevel]
FROM [Employees]