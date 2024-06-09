USE [Bank]

CREATE OR ALTER TRIGGER tr_SetIsDeletedOnDelete
ON AccountHolders
INSTEAD OF DELETE
AS
 UPDATE AccountHolders SET IsDeleted = 1
 WHERE Id IN (SELECT Id FROM deleted)
GO