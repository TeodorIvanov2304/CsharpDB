USE [Bank]
--01. Create Table Logs

--Create Table Logs
CREATE TABLE [Logs] 
(
	[Id] INT PRIMARY KEY IDENTITY,
	[AccountId] INT NOT NULL FOREIGN KEY REFERENCES Accounts(Id),
	[OldSum] MONEY NOT NULL,
	[NewSum] MONEY NOT NULL

)

--Create trigger

		CREATE OR ALTER TRIGGER tr_LogAccountChange
			ON [Accounts]
		   FOR UPDATE
			AS
		INSERT INTO [Logs] ([AccountId],[NewSum],[OldSum])
		SELECT i.Id,i.Balance,d.Balance 
		  FROM inserted AS i
		  JOIN deleted AS d ON i.Id = d.Id
		 WHERE i.Balance != d.Balance
            GO

--Test
		UPDATE [Accounts]
		   SET [Balance] += 1000
		 WHERE [Id] = 8

		SELECT * FROM [Logs]


--02.Create Table Emails


--Create Table
CREATE TABLE [NotificationEmails] 
(
	[Id] INT PRIMARY KEY IDENTITY,
	[Recipient] INT NOT NULL,
	[Subject] NVARCHAR(MAX) NOT NULL,
	[Body] NVARCHAR(MAX) NOT NULL
)


--Create Trigger

		CREATE OR ALTER TRIGGER tr_createNewEmailWhenNewRecordIsInserted
			ON [Logs] 
		   FOR INSERT
			AS
		INSERT INTO [NotificationEmails] ([Recipient],[Subject],[Body])
		SELECT  i.AccountId,  
		        CONCAT_WS(' ','Balance change for account:',i.AccountId) AS [Subject], 
				CONCAT_WS(' ','On',GETDATE(),'your balance was changed from',i.OldSum, 'to',i.NewSum ) AS [Body]
		  FROM inserted AS i
			GO


--Test
			UPDATE [Accounts]
		   SET [Balance] += 1000
		 WHERE [Id] = 10
		
		SELECT * FROM [NotificationEmails]

--03.Deposit Money
CREATE PROCEDURE usp_DepositMoney(@AccountId INT, @MoneyAmount DECIMAL(10,4))
AS
  IF @MoneyAmount > 0 
  BEGIN
	   UPDATE [Accounts]
	   SET [Balance] += @MoneyAmount
	   WHERE [Id] = @AccountId
    END

EXEC usp_DepositMoney 1,10

--04.Withdraw Money Procedure

CREATE PROCEDURE usp_WithdrawMoney (@AccountId INT, @MoneyAmount DECIMAL(10,4))
AS
IF @MoneyAmount > 0
BEGIN
	UPDATE [Accounts]
	SET [Balance] -=@MoneyAmount
WHERE [Id] = @AccountId
END
GO


--05.Money Transfer

