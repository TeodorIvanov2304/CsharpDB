--Substring takes only part of a string
   SELECT 
CONCAT_WS(' ',SUBSTRING(FirstName,1,2),SUBSTRING(LastName,1,2))
       AS [Initials]
     FROM [Employees]