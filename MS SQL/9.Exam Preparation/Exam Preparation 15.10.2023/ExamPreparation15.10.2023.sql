--Tourist Agency

--Section 1. DDL (30 pts)
CREATE DATABASE [TouristAgency]

USE [TouristAgency]

GO
CREATE TABLE Countries
(
	Id INT PRIMARY KEY IDENTITY,
	[Name] NVARCHAR(50) NOT NULL
)

CREATE TABLE Destinations
(
	Id INT PRIMARY KEY IDENTITY,
	[Name] VARCHAR(50) NOT NULL,
	CountryId INT FOREIGN KEY REFERENCES Countries(Id) NOT NULL
)

CREATE TABLE Rooms
(
	Id INT PRIMARY KEY IDENTITY,
	[Type] VARCHAR(40) NOT NULL,
	Price DECIMAL(18,2) NOT NULL,
	BedCount INT NOT NULL
	    --Add constraint while creating table
		CHECK(BedCount > 0 AND BedCount <= 10)
)

CREATE TABLE Hotels
(
	Id INT PRIMARY KEY IDENTITY,
	[Name] VARCHAR(50) NOT NULL,
	DestinationId INT FOREIGN KEY REFERENCES Destinations(Id) NOT NULL
)

CREATE TABLE Tourists
(
	Id INT PRIMARY KEY IDENTITY,
	[Name] NVARCHAR(80) NOT NULL,
	PhoneNumber VARCHAR(20) NOT NULL,
	Email VARCHAR(80),
	CountryId INT FOREIGN KEY REFERENCES Countries(Id)
)

CREATE TABLE Bookings
(
	Id INT PRIMARY KEY IDENTITY,
	ArrivalDate DATETIME2 NOT NULL,
	DepartureDate DATETIME2 NOT NULL,
	AdultsCount INT NOT NULL
		CHECK(AdultsCount BETWEEN 1 AND 10),
		--AdultsCount > =1 AND AdultsCount < =10
	ChildrenCount INT NOT NULL
		CHECK(ChildrenCount BETWEEN 0 AND 9),
	TouristId INT FOREIGN KEY REFERENCES Tourists(Id) NOT NULL,
	HotelId INT FOREIGN KEY REFERENCES Hotels(Id) NOT NULL,
	RoomId INT FOREIGN KEY REFERENCES Rooms(Id) NOT NULL
)


--Mapping table
CREATE TABLE HotelsRooms
(
	HotelId INT NOT NULL FOREIGN KEY (HotelId) REFERENCES Hotels(Id),
	RoomId INT NOT NULL FOREIGN KEY (RoomId) REFERENCES Rooms(Id)
	CONSTRAINT PK_HotelsRooms PRIMARY KEY(HotelId,RoomId),
)

GO
--Section 2. DML (10 pts)
--First you have to import Dataset sql

INSERT INTO Tourists ([Name],PhoneNumber,Email,CountryId)
	 VALUES ('John Rivers','653-551-1555','john.rivers@example.com',6),
	   	      ('Adeline Aglaé','122-654-8726','adeline.aglae@example.com',2),
	   	      ('Sergio Ramirez','233-465-2876','s.ramirez@example.com',3),
	   	      ('Johan Müller','322-876-9826','j.muller@example.com',7),
	   	      ('Eden Smith','551-874-2234','eden.smith@example.com',6)

INSERT INTO Bookings (ArrivalDate,DepartureDate,AdultsCount,ChildrenCount,TouristId,HotelId,RoomId)
	 VALUES ('2024-03-01','2024-03-11', 1, 0, 21, 3, 5),
	        ('2023-12-28','2024-01-06', 2, 1, 22, 13, 3),
			('2023-11-15','2023-11-20', 1, 2, 23, 19, 7),
			('2023-12-05','2023-12-09', 4, 0, 24, 6, 4),
			('2024-05-01','2024-05-07', 6, 0 ,25 ,14 ,6)

GO

--3.Update
        UPDATE Bookings
           SET DepartureDate = DATEADD (DAY,1,DepartureDate) 
         WHERE ArrivalDate > = '2023-12-01' AND ArrivalDate < '2024-01-01'

		UPDATE [Tourists]
		   SET Email = NULL
		 WHERE Email LIKE '%ma%' 

--4.Delete
BEGIN TRANSACTION

DECLARE @TouristToDelete TABLE (Id INT)

INSERT INTO @TouristToDelete(Id)
SELECT Id
  FROM Tourists
 WHERE [Name] LIKE '%Smith%'

DELETE FROM Bookings
WHERE TouristId IN (SELECT Id FROM @TouristToDelete)

DELETE FROM Tourists
 WHERE Id IN (SELECT Id FROM @touristToDelete)

COMMIT

--Section 3. Querying (40 pts)
--You need to start with a fresh dataset, so recreate your DB and import the sample data again ("Dataset.sql").

--5.Bookings by Price of Room and Arrival Date

SELECT 
       FORMAT(b.ArrivalDate,'yyyy-MM-dd','bg-BG') AS ArrivalDate,
	   b.AdultsCount,
	   b.ChildrenCount
 FROM  Bookings AS b
 JOIN  Rooms AS r ON b.RoomId = r.Id
ORDER  BY r.Price DESC, b.ArrivalDate

--6.Hotels by Count of Bookings

SELECT Id,[Name]

FROM
(
	SELECT 
		   COUNT(b.HotelId) AS [Count], 
		   h.Id,
		   h.[Name]
	  FROM Hotels AS h
	  JOIN HotelsRooms AS hr ON h.Id = hr.HotelId
	  JOIN Rooms AS r ON hr.RoomId = r.Id
	  JOIN Bookings AS b ON h.Id = b.HotelId
	 GROUP BY h.Id,h.[Name],r.[Type]
	HAVING r.[Type] = 'VIP Apartment'
) 
        AS datt
  ORDER BY [Count] DESC

--6. Doncho Georgiev solution
	SELECT 
		   h.Id,
		   h.[Name]
	  FROM Hotels AS h
	  JOIN HotelsRooms AS hr ON h.Id = hr.HotelId
	  JOIN Rooms AS r ON hr.RoomId = r.Id
	  JOIN Bookings AS b ON h.Id = b.HotelId AND r.[Type] = 'VIP Apartment'
	 GROUP BY h.Id,h.[Name],r.[Type]
	 ORDER BY COUNT(*) DESC


--7.Tourists without Bookings
SELECT 
       t.Id,
	   t.[Name],
	   t.PhoneNumber
  FROM Tourists AS t
  LEFT JOIN Bookings AS b ON t.Id = b.TouristId
 WHERE t.Id NOT IN (SELECT TouristId FROM Bookings)
 ORDER BY [Name]

--7 Without JOIN
SELECT 
       Id,
	   [Name],
	   PhoneNumber
  FROM Tourists
 WHERE Id NOT IN (SELECT TouristId FROM Bookings)
 ORDER BY [Name]

--8.First 10 Bookings

SELECT TOP 10
	   h.[Name] AS HotelName,
	   d.[Name] AS DestinationName,
	   c.[Name] AS CountryName
 FROM  Bookings AS b
 JOIN  Hotels AS h ON b.HotelId = h.Id
 JOIN  Destinations AS d ON h.DestinationId = d.Id
 JOIN  Countries AS c ON d.CountryId = c.Id
WHERE  ArrivalDate < '2023-12-31' AND HotelId % 2 = 1
ORDER  BY c.[Name],b.ArrivalDate

--9.Tourists booked in Hotels
SELECT 
	    h.[Name] AS [HotelName],
	    r.[Price] AS [RoomPrice]
  FROM  [Tourists] AS t
  JOIN  [Bookings] AS b ON t.[Id] = b.[TouristId]
  JOIN  [Hotels] AS h ON b.[HotelId] = h.[Id]
  JOIN  [Rooms] AS r ON b.[RoomId] = r.[Id]
 WHERE  t.[Name] NOT LIKE '%EZ'
 ORDER  BY r.[Price] DESC

--10.Hotels Revenue


SELECT DISTINCT
	   h.[Name] AS HotelName,
	   SUM(r.Price * DATEDIFF(DAY,b.ArrivalDate,b.DepartureDate)) AS TotalRevenue 
  FROM Bookings AS b
  JOIN Hotels AS h ON b.HotelId = h.Id
  JOIN Rooms AS r ON b.RoomId = r.Id
 GROUP BY h.[Name]
 ORDER BY TotalRevenue DESC

---11.Rooms with Tourists

CREATE FUNCTION udf_RoomsWithTourists(@name VARCHAR(40)) 
RETURNS INT
AS

BEGIN
DECLARE @peopleCount INT 
SET @peopleCount = 
(
	SELECT
		   SUM(b.AdultsCount) + SUM(b.ChildrenCount) AS [TouristCount]
	  FROM Bookings AS b
	  JOIN Rooms AS r ON b.RoomId = r.Id
	 WHERE r.[Type] = @name
 )
    RETURN @peopleCount 
END

SELECT dbo.udf_RoomsWithTourists('Triple Room') AS [TouristCount]


--12.Search for Tourists from a Specific Country

CREATE PROCEDURE usp_SearchByCountry(@country NVARCHAR (50)) 
AS
BEGIN
SELECT 
       t.[Name],
	   t.PhoneNumber,
	   t.Email,
	   COUNT(b.TouristId) AS CountOfBookings
  FROM Tourists AS t
  JOIN Bookings AS b ON t.Id = b.TouristId
  JOIN Countries AS c ON t.CountryId = c.Id
 WHERE c.[Name] = @country
 GROUP BY t.[Name],t.PhoneNumber,t.Email
 ORDER BY t.[Name],CountOfBookings
 END

EXEC usp_SearchByCountry 'Croatia'
