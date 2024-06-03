USE [Gringotts]


--1.Records' Count
SELECT 
       COUNT(*) AS [Count]
  FROM [WizzardDeposits]


--2.Longest Magic Wand
SELECT 
       MAX(wd.[MagicWandSize]) AS [LongestMagicWand]
  FROM [WizzardDeposits] wd

--3.Longest Magic Wand Per Deposit Groups

SELECT 
	   wd.[DepositGroup],
       MAX(wd.[MagicWandSize]) AS [LongestMagicWand]
  FROM [WizzardDeposits] wd
 GROUP BY [DepositGroup]

 
--3. With ROW_NUMBER

SELECT DepositGroup, MagicWandSize
FROM
(SELECT 
	DepositGroup, MagicWandSize,
	ROW_NUMBER() OVER 
		(PARTITION BY DepositGroup ORDER BY MagicWandSize DESC) AS RankedMagicWands
FROM WizzardDeposits) AS SubQuery
WHERE RankedMagicWands = 1

--4.Smallest Deposit Group Per Magic Wand Size
-- We can use Aggregate funciton in the ORDER BY Clause!!!
SELECT TOP 2
	   wd.[DepositGroup]
  FROM [WizzardDeposits] wd
 GROUP BY wd.[DepositGroup]
 ORDER BY AVG(wd.[MagicWandSize])


--5.Deposits Sum

SELECT 
       wd.[DepositGroup],
	   SUM(wd.DepositAmount) AS [TotalSum]
  FROM [WizzardDeposits] AS wd
 GROUP BY wd.[DepositGroup]

--6.Deposits Sum for Ollivander Family

SELECT 
	 wd.DepositGroup,
	 SUM(wd.[DepositAmount]) AS [TotalSum]
FROM [WizzardDeposits] AS wd
WHERE wd.[MagicWandCreator] = 'Ollivander family'
GROUP BY wd.[DepositGroup]

--7.Deposits Filter
SELECT 
	 wd.DepositGroup,
	 SUM(wd.[DepositAmount]) AS [TotalSum]
FROM [WizzardDeposits] AS wd
WHERE wd.[MagicWandCreator] = 'Ollivander family'
GROUP BY wd.[DepositGroup]
HAVING SUM(wd.[DepositAmount]) < 150000
ORDER BY [TotalSum] DESC

--8.Deposit Charge
SELECT 
       wd.[DepositGroup],
	   wd.[MagicWandCreator],
	   MIN(wd.[DepositCharge]) AS [MinDepositCharge]
  FROM [WizzardDeposits] AS wd
 GROUP BY wd.[DepositGroup], wd.[MagicWandCreator]


--9.Age Groups

SELECT [AgeGroup],COUNT(*)  AS [WizardCount] FROM
(
SELECT 
  CASE 
	   WHEN wd.[Age] BETWEEN 0 AND 10 THEN '[0-10]'
	   WHEN wd.[Age] BETWEEN 11 AND 20 THEN '[11-20]'
	   WHEN wd.[Age] BETWEEN 21 AND 30 THEN '[21-30]'
	   WHEN wd.[Age] BETWEEN 31 AND 40 THEN '[31-40]'
	   WHEN wd.[Age] BETWEEN 41 AND 50 THEN '[41-50]'
	   WHEN wd.[Age] BETWEEN 51 AND 60 THEN '[51-60]'
	   WHEN wd.[Age] > 60 THEN '[61+]'
	   END AS [AgeGroup]
  FROM [WizzardDeposits] AS wd
) AS [NestedQuery]
GROUP BY [AgeGroup]


--10.First Letter
SELECT [FirstLetter] FROM
(
	SELECT 
		   LEFT(wd.[FirstName],1) AS [FirstLetter]
	  FROM  [WizzardDeposits] AS wd
	 WHERE wd.[DepositGroup] = 'Troll Chest'
)       AS dt
GROUP BY [FirstLetter]


--11. Average Interest 
SELECT 
     wd.[DepositGroup],
	 wd.[IsDepositExpired],
	 AVG(wd.[DepositInterest]) AS [AverageInterestRate]
FROM [WizzardDeposits] AS wd
WHERE wd.[DepositStartDate] > '01/01/1985'
GROUP BY wd.[DepositGroup],wd.[IsDepositExpired]
ORDER BY wd.[DepositGroup] DESC, wd.[IsDepositExpired]

--12.*Rich Wizard, Poor Wizard

SELECT ABS(SUM([Difference])) AS [SumDifference] FROM 
(

		SELECT 
			   wd.[FirstName] AS [Host Wizard],
			   wd.[DepositAmount] AS [Host Wizard Deposit],
			   wd2.[FirstName] AS [Guest Wizard],
			   wd2.[DepositAmount] AS [Guest Wizard Deposit],
			   wd.[DepositAmount] - wd2.[DepositAmount] AS [Difference]
		 FROM [WizzardDeposits] wd
		 JOIN [WizzardDeposits] AS wd2 ON wd.Id = wd2.Id + 1
) AS [Data]


--Withe LEAD
SELECT SUM([Difference]) AS SumDifference
FROM
(
SELECT 
	FirstName AS HostWizzard,
	DepositAmount AS HostWizzardDeposit,
	LEAD(FirstName) OVER (ORDER BY Id) AS GuestWizzard,
	LEAD([DepositAmount]) OVER (ORDER BY Id) AS GuestWizzardDeposit,
	(DepositAmount - LEAD([DepositAmount]) OVER (ORDER BY Id)) AS [Difference]
FROM WizzardDeposits
) AS SubQuery


--13.Departments Total Salaries
USE [SoftUni]

SELECT 
        e.[DepartmentID],
	    SUM(e.[Salary]) AS [TotalSalary]
  FROM  [Employees] AS e
 GROUP  BY e.[DepartmentID]


--14.Employees Minimum Salaries
   SELECT e.[DepartmentID],
          MIN(e.[Salary]) AS [MinimumSalary]
     FROM [Employees] AS e
    WHERE e.[DepartmentID] IN (2,5,7) AND e.[HireDate] > '01/01/2000'
 GROUP BY e.[DepartmentID]



--15.Employees Average Salaries

--With SELECT * INTO we create new table in SoftUni database and insert all the desired info into the new table

SELECT * INTO [RichEmployees]
  FROM [Employees]
 WHERE [Salary] > 30000

DELETE 
  FROM [RichEmployees]
 WHERE [ManagerID] = 42

UPDATE [RichEmployees]
SET [Salary] += 5000
WHERE [DepartmentID] = 1

SELECT 
      [DepartmentID],
	  AVG([Salary]) AS [AverageSalary]
 FROM [RichEmployees]
GROUP BY[DepartmentID]


--16.Employees Maximum Salaries

SELECT * FROM
(
	SELECT 
		  e.[DepartmentID],
		  MAX(e.[Salary]) AS [MaxSalary]
	 FROM [Employees] AS e
	GROUP BY e.[DepartmentID]
) AS [Data]
    WHERE [MaxSalary] NOT BETWEEN 30000 AND 70000

--With HAVING
SELECT 
		  e.[DepartmentID],
		  MAX(e.[Salary]) AS [MaxSalary]
	 FROM [Employees] AS e
	GROUP BY e.[DepartmentID]
   HAVING MAX(e.[Salary]) NOT BETWEEN 30000 AND 70000

--17.Employees Count Salaries

SELECT COUNT(*) AS [Count]
  FROM [Employees]
 WHERE [ManagerID] IS NULL


--18.*3rd Highest Salary

    SELECT DISTINCT [DepartmentID], [Salary] AS [ThirdHighestSalary]
      FROM
(
	SELECT 
		   [DepartmentID],
		   [Salary],
		   DENSE_RANK() OVER (PARTITION BY [DepartmentID] ORDER BY Salary DESC) AS [Rank]
	  FROM [Employees]

)       AS [Data]
     WHERE [Rank] = 3

--19.**Salary Challenge

WITH DepartmentAvarageSalaries AS
(
	SELECT 
		DepartmentID, AVG(Salary) AS AvarageSalary
	FROM Employees
	GROUP BY DepartmentID
)
SELECT TOP 10
	FirstName, LastName, e.DepartmentID
FROM Employees AS e
JOIN DepartmentAvarageSalaries AS das ON das.DepartmentID = e.DepartmentID
WHERE e.Salary > das.AvarageSalary
ORDER BY e.DepartmentID

