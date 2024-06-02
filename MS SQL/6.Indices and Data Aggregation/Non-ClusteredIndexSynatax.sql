--Non-clustered index synatx
CREATE NONCLUSTERED INDEX
IX_Employees_FirstName_LastName
ON [Employees]([FirstName],[LastName])