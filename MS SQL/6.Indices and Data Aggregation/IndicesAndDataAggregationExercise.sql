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
	   MIN(wd.[DepositCharge])
  FROM [WizzardDeposits] AS wd
 GROUP BY wd.[DepositGroup], wd.[MagicWandCreator]

SELECT * FROM [WizzardDeposits]
ORDER BY [MagicWandCreator]
