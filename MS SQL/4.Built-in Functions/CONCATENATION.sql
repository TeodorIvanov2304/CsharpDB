USE [SoftUni]

SELECT 
CONCAT(FirstName,' ',MiddleName,' ', LastName)
    AS [Full Name]
  FROM [Employees]

--Concat With Separator

   SELECT 
CONCAT_WS(' ',FirstName,MiddleName,LastName)
       AS [Full Name]
     FROM [Employees]