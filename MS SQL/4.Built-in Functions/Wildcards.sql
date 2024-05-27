--Example: Find all employees who's first name starts with "Ro"
-- Supported characters include:
-- % -- any string, including zero-length
-- _ -- any single character
-- […] -- any character within range
-- [^…] -- any character not in the range
USE [SoftUni]

SELECT [FirstName],[LastName]
  FROM [Employees]
 WHERE [FirstName] LIKE 'Ro%'

 --Finds all names with second and third letter te
 SELECT [FirstName],[LastName]
  FROM [Employees]
 WHERE [FirstName] LIKE '_te%'

  --Finds all names with third and fourth letter te
  SELECT [FirstName],[LastName]
  FROM [Employees]
 WHERE [FirstName] LIKE '__te%'


 --ESCAPE
SELECT [ID], [Name]
FROM [Tracks]
WHERE [Name] LIKE '%max!%' ESCAPE '!'