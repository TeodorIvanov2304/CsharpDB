USE [Bank]
CREATE TRIGGER tr_AddToLogsOnAccountUpdate
ON Accounts FOR UPDATE
AS
 INSERT INTO Logs(AccountId, OldAmount, NewAmount, UpdatedOn)
 SELECT i.Id, d.Balance, i.Balance, GETDATE()
 FROM inserted AS i
 JOIN deleted AS d ON i.Id = d.Id
 WHERE i.Balance != d.Balance
GO