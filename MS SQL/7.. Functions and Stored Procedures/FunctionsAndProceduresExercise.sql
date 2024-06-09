--1.Employees with Salary Above 35000

USE [SoftUni]

GO
CREATE PROCEDURE dbo.usp_GetEmployeesSalaryAbove35000 AS
 BEGIN
SELECT 
	   [FirstName],
	   [LastName]
  FROM [Employees]
 WHERE [Salary] > 35000
   END
GO

EXEC dbo.usp_GetEmployeesSalaryAbove35000


--2.Employees with Salary Above Number
GO
CREATE  PROCEDURE usp_GetEmployeesSalaryAboveNumber
(@number DECIMAL(18,4)) 
AS 

SELECT 
      [FirstName],
	  [LastName]
 FROM [Employees]
 WHERE [Salary] > = @number
GO

EXECUTE usp_GetEmployeesSalaryAboveNumber 40000

--3.Town Names Starting With
GO
CREATE OR ALTER PROCEDURE usp_GetTownsStartingWith 
(@startingLetter VARCHAR(MAX))
AS
BEGIN
SELECT 
     [Name] AS [Town]
FROM [Towns]
WHERE LEFT([Name],LEN(@startingLetter)) = @startingLetter
END
GO
											
EXEC dbo.usp_GetTownsStartingWith Bo


--4.Employees from Town

GO
CREATE PROCEDURE usp_GetEmployeesFromTown  (@townName VARCHAR(MAX))
AS
BEGIN
SELECT 
       e.[FirstName],
	   e.[LastName]
  FROM [Employees] AS e
  JOIN [Addresses] AS a ON e.AddressID = a.AddressID
  JOIN [Towns] AS t ON a.TownID = t.TownID
WHERE [Name] = @townName
END
GO

EXECUTE usp_GetEmployeesFromTown Sofia

--5.Salary Level Function
CREATE FUNCTION ufn_GetSalaryLevel(@salary DECIMAL(18,4)) 
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

EXECUTE dbo.ufn_GetSalaryLevel 10000

SELECT FirstName, LastName,dbo.ufn_GetSalaryLevel (50000)
FROM [Employees]



--6.Employees by Salary Level

CREATE PROCEDURE usp_EmployeesBySalaryLevel @salaryLevel NVARCHAR(10)
AS
BEGIN
    SELECT [FirstName],[LastName]
	FROM [Employees]
	--Using the previously saved function
	WHERE dbo.ufn_GetSalaryLevel(Salary) = @salaryLevel 
END

EXEC usp_EmployeesBySalaryLevel 'high'


--7.Define Function
CREATE FUNCTION ufn_IsWordComprised (@setOfLetters NVARCHAR(MAX), @word NVARCHAR(MAX))
			RETURNS BIT
			     AS

              BEGIN
				  DECLARE @wordLength INT = LEN(@word)
				  DECLARE @iterator INT = 1

				    WHILE(@iterator <= @wordLength)
              BEGIN 
					   IF(CHARINDEX (SUBSTRING(@word,@iterator,1),@setOfLetters) = 0)
				   RETURN 0
					  SET @iterator += 1
                END
			       RETURN 1
                END

--8.Delete Employees and Departments
CREATE PROC usp_DeleteEmployeesFromDepartment (@departmentId INT) 
		 AS
				 DECLARE @deletedEmployees TABLE ([Id] INT)
	
			 INSERT INTO @deletedEmployees
				  SELECT [EmployeeID]
				    FROM [Employees]
				   WHERE [DepartmentID] = @departmentId

			 DELETE FROM [EmployeesProjects]
			       WHERE [EmployeeID] IN (SELECT * FROM @deletedEmployees)

			       ALTER TABLE [Departments]
			       ALTER COLUMN [ManagerID] INT

				  UPDATE [Departments]
					 SET [ManagerID] = NULL
				   WHERE [ManagerID] IN (SELECT * FROM @deletedEmployees)

				  UPDATE [Employees]
				     SET [ManagerID] = NULL
				   WHERE [ManagerID] IN (SELECT * FROM @deletedEmployees)

			 DELETE FROM [Employees]
				   WHERE [DepartmentID] = @departmentId

			 DELETE FROM [Departments]
				   WHERE [DepartmentID] = @departmentId

				  SELECT COUNT(*) AS [Count] 
					FROM [Employees]
				   WHERE [DepartmentID] = @departmentId

				   
--9.Find Full Name
--USE [Bank]

CREATE PROCEDURE usp_GetHoldersFullName
              AS
		   BEGIN
		  SELECT 
			      CONCAT_WS(' ',[FirstName],[LastName]) AS [Fullame]
			FROM [AccountHolders]
			 END
EXECUTE usp_GetHoldersFullName
			  SELECT * FROM [Accounts]
			  

--10.People with Balance Higher Than

CREATE PROCEDURE usp_GetHoldersWithBalanceHigherThan (@number MONEY)
           AS
        BEGIN
	   SELECT 
			   ah.[FirstName],
			   ah.[LastName]
		 FROM  [AccountHolders] AS ah
		 JOIN  [Accounts] AS a ON a.[AccountHolderId] = ah.[Id]
		GROUP  BY ah.[FirstName],ah.[LastName]
	   HAVING  SUM(a.[Balance]) > @number
	    ORDER  BY ah.[FirstName], ah.[LastName]
	      END  

 
      EXEC usp_GetHoldersWithBalanceHigherThan 100000


--11.Future Value Function

CREATE FUNCTION ufn_CalculateFutureValue  (@initialSum DECIMAL(10,4),@yearliInterestRate FLOAT,@numberOfYears INT)
     RETURNS DECIMAL(10,4)
          AS 
       BEGIN
	  RETURN @initialSum * (POWER((1 + @yearliInterestRate),@numberOfYears))
         END


SELECT   dbo.ufn_CalculateFutureValue (10000,0.1,5)

--12.Calculating Interest

CREATE PROCEDURE usp_CalculateFutureValueForAccount (@accountId INT,@interestRate FLOAT)
			     AS
			DECLARE @term INT = 5
			 SELECT 
					 ah.[Id] AS [AccountId],
					 ah.[FirstName] AS [First Name],
					 ah.[LastName] AS [Last Name],
					 a.[Balance] AS [Current Balance], 
					 dbo.ufn_CalculateFutureValue ([Balance],@interestRate,@term) AS [Balance in 5 years]
			   FROM  [AccountHolders] AS ah
			   JOIN  [Accounts] AS a ON ah.[Id] = a.[AccountHolderId]
			  WHERE  a.[Id] = @accountId
					 
EXECUTE dbo.usp_CalculateFutureValueForAccount 1,0.1

--13.*Scalar Function: Cash in User Games Odd Rows
USE [Diablo]

CREATE FUNCTION ufn_CashInUsersGames (@gameName VARCHAR(MAX))
			  RETURNS TABLE
			       AS
			   RETURN
      (
			   SELECT SUM([Cash]) AS [SumCash]
			     FROM
		   (
			   SELECT 
					   g.[Id],
					   g.[Name],
					   ug.[Cash],
					   ROW_NUMBER() OVER(ORDER BY [Cash] DESC) AS [RowRank]
			     FROM [Games] AS g
			     JOIN [UsersGames] AS ug ON g.Id = ug.GameId
			    WHERE g.[Name] = @gameName
			)      AS dt
			    WHERE [RowRank] % 2 = 1
       )