CREATE DATABASE Accounting 

GO

USE Accounting


--Section 1. DDL (30 pts)

CREATE TABLE Countries
(
	Id INT PRIMARY KEY IDENTITY,
	[Name] VARCHAR(10) NOT NULL
)

CREATE TABLE Addresses
(
	Id INT PRIMARY KEY IDENTITY,
	StreetName NVARCHAR(20) NOT NULL,
	StreetNumber INT,
	PostCode INT NOT NULL,
	City VARCHAR(30) NOT NULL,
	CountryId INT NOT NULL FOREIGN KEY REFERENCES Countries(Id)
)

CREATE TABLE Vendors
(
	Id INT PRIMARY KEY IDENTITY,
	[Name] NVARCHAR(25) NOT NULL,
	NumberVAT NVARCHAR(15) NOT NULL,
	AddressId INT NOT NULL FOREIGN KEY REFERENCES Addresses(Id)
)

CREATE TABLE Clients
(
	Id INT PRIMARY KEY IDENTITY,
	[Name] NVARCHAR(25) NOT NULL,
	NumberVAT NVARCHAR(15) NOT NULL,
	AddressId INT NOT NULL FOREIGN KEY REFERENCES Addresses(Id)
)

CREATE TABLE Categories
(
	Id INT PRIMARY KEY IDENTITY,
	[Name] VARCHAR(10) NOT NULL
)

CREATE TABLE Products
(
	Id INT PRIMARY KEY IDENTITY,
	[Name] NVARCHAR(35) NOT NULL,
	Price DECIMAL(18,2) NOT NULL,
	CategoryId INT NOT NULL FOREIGN KEY REFERENCES Categories(Id),
	VendorId INT NOT NULL FOREIGN KEY REFERENCES Vendors(Id)
)

CREATE TABLE Invoices
(
	Id INT PRIMARY KEY IDENTITY,
	Number INT NOT NULL UNIQUE,
	IssueDate DATETIME2 NOT NULL,
	DueDate DATETIME2 NOT NULL,
	Amount DECIMAL(18,2) NOT NULL,
	Currency VARCHAR(5) NOT NULL,
	ClientId INT NOT NULL FOREIGN KEY REFERENCES Clients(Id)
)

CREATE TABLE ProductsClients
(
	ProductId INT NOT NULL FOREIGN KEY REFERENCES Products(Id),
	ClientId INT NOT NULL FOREIGN KEY REFERENCES Clients(Id)
	CONSTRAINT PK_ProductsClients PRIMARY KEY(ProductId,ClientId)
)

GO

--Section 2. DML (10 pts)

INSERT INTO Products([Name],Price,CategoryId,VendorId)
     VALUES ('SCANIA Oil Filter XD01',78.69,1, 1),
	        ('MAN Air Filter XD01'	,97.38,	1, 5),
			('DAF Light Bulb 05FG87',55.00, 2, 13),
			('ADR Shoes 47-47.5',49.85, 3, 5),
			('Anti-slip pads S', 5.87, 5,7)

INSERT INTO Invoices(Number,IssueDate,DueDate,Amount,Currency,ClientId)
	 VALUES (1219992181, '2023-03-01', '2023-04-30', 180.96,'BGN',3),
	        (1729252340, '2022-11-06','2023-01-04',158.18,'EUR',13),
			(1950101013, '2023-02-17','2023-04-18',	615.15,	'USD', 19)



--3.Update

UPDATE Invoices
   SET DueDate = '2023-04-01'
 WHERE IssueDate >= '2022-11-01' AND IssueDate <= '2022-11-30'

UPDATE Clients
   SET AddressId = 3
 WHERE [Name] LIKE '%CO%'

--UPDATE Invoices Second Solution
UPDATE Invoices
SET DueDate = '2023-04-01'
WHERE Year(IssueDate) = 2022 AND Month(IssueDate) = 11

--4. Delete

BEGIN TRANSACTION

DECLARE @clientsToDelete TABLE (Id INT)
INSERT INTO @clientsToDelete(Id)
	 SELECT Id
       FROM Clients
	  WHERE NumberVAT LIKE 'IT%'

DELETE FROM ProductsClients
WHERE ClientId IN (SELECT Id FROM @clientsToDelete)

DELETE FROM Invoices
WHERE ClientId IN (SELECT Id FROM @clientsToDelete)

DELETE FROM Clients
WHERE Id IN (SELECT Id FROM @clientsToDelete)

COMMIT

--Section 3. Querying (40 pts)
--5.Invoices by Amount and Date

SELECT 
	   Number,
	   Currency
  FROM Invoices
 ORDER BY Amount DESC, DueDate 


--6.Products by Category

SELECT 
       p.Id,
	   p.[Name],
	   p.Price,
	   c.[Name] AS CategoryName
  FROM Products AS p
  JOIN Categories AS c ON p.CategoryId = c.Id
 WHERE c.[Name] IN ('ADR','Others')
 ORDER BY Price DESC


--7.Clients without Products

SELECT 
		   c.Id,
		   c.[Name],
		   CONCAT(a.StreetName,' ', a.StreetNumber,', ',  a.City, ', ', a.PostCode, ', ', co.[Name]) AS c
	  FROM Clients AS c
	  JOIN Addresses AS a ON c.AddressId = a.Id
	  JOIN Countries AS co ON a.CountryId = co.Id
      LEFT JOIN ProductsClients AS pc ON c.Id = pc.ClientId
	  WHERE c.Id NOT IN (SELECT ClientId FROM ProductsClients)
	  ORDER BY c.[Name]
  
--8.First 7 Invoices

SELECT TOP 7
       i.Number,
	   i.Amount,
	   c.[Name]
  FROM Invoices AS i
  JOIN Clients AS c ON i.ClientId = c.Id
  WHERE i.IssueDate < '2023-01-01' AND i.Currency = 'EUR' OR i.Amount > 500.00 AND c.NumberVAT LIKE 'DE%'
 ORDER BY i.Number,i.Amount DESC

--9.Clients with VAT

 SELECT
	    c.[Name],
        MAX(p.Price),
		c.NumberVAT
  FROM Clients AS c
  JOIN ProductsClients AS pc ON c.Id = pc.ClientId
  JOIN Products AS p ON pc.ProductId = p.Id
  WHERE c.[Name] NOT LIKE '%KG' 
  GROUP BY c.[Name],c.NumberVAT
  ORDER BY MAX(p.Price) DESC 


--10.Clients by Price

SELECT 
      c.[Name] AS Client,
	  FLOOR(AVG(p.Price)) AS AvgPrice
 FROM Clients AS c
 JOIN ProductsClients AS pc ON c.Id = pc.ClientId
 JOIN Products AS p ON pc.ProductId = p.Id
 JOIN Vendors AS v ON p.VendorId = v.Id
WHERE c.Id  IN (SELECT ClientId FROM ProductsClients) AND v.NumberVAT LIKE '%FR%'
GROUP BY c.[Name] 
ORDER BY FLOOR(AVG(p.Price)),c.[Name] DESC

--Section 4. Programmability (20 pts)

--11.Product with Clients

CREATE FUNCTION udf_ProductWithClients(@name VARCHAR(35)) 
RETURNS INT
AS
BEGIN
DECLARE @productCount INT
SET @productCount = 
(
SELECT 
       COUNT(c.[Name]) 
  FROM Clients AS c
  JOIN ProductsClients AS pc ON c.Id = pc.ClientId
  JOIN Products AS p ON pc.ProductId = p.Id
  WHERE p.[Name] = @name
)
RETURN @productCount
END

SELECT dbo.udf_ProductWithClients('DAF FILTER HU12103X')

--12.Search for Vendors from a Specific Country

CREATE PROCEDURE usp_SearchByCountry(@country VARCHAR(10)) 
AS
BEGIN
SELECT 
       v.[Name] AS Vendor,
	   v.NumberVAT AS VAT,
	   CONCAT_WS(' ',a.StreetName,a.StreetNumber) AS StreetInfo,
	   CONCAT_WS(' ',a.City,a.PostCode) AS CityInfo
  FROM Vendors AS v
  JOIN Addresses AS a ON v.AddressId = a.Id
  JOIN Countries AS c ON a.CountryId = c.Id
 WHERE c.[Name] = @country
 ORDER BY v.[Name],a.City
END

EXEC usp_SearchByCountry 'France'
