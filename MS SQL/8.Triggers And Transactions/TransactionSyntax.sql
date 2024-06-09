CREATE PROC usp_Withdraw (@withdrawAmount DECIMAL(18,2), @accountId
INT)
AS
BEGIN TRANSACTION
UPDATE Accounts SET Balance = Balance - @withdrawAmount
WHERE Id = @accountId
IF @@ROWCOUNT <> 1 -- Didn’t affect exactly one row
BEGIN
 ROLLBACK
 THROW 50001, 'Invalid account!', 1
 RETURN
END
COMMIT
