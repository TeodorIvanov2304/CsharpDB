CREATE DATABASE RailwaysDb

USE RailwaysDb
--Section 1.DDL (30 pts)
--Create a database called RailwaysDb. You need to create 7 tables:

CREATE TABLE Passengers
(
	Id INT NOT NULL PRIMARY KEY IDENTITY,
	[Name] NVARCHAR(80) NOT NULL
)

CREATE TABLE Towns
(
	Id INT NOT NULL PRIMARY KEY IDENTITY,
	[Name] VARCHAR(30) NOT NULL
)

CREATE TABLE RailwayStations
(
	Id INT NOT NULL PRIMARY KEY IDENTITY,
	[Name] VARCHAR(50) NOT NULL,
	TownId INT NOT NULL FOREIGN KEY REFERENCES Towns(Id)
)

CREATE TABLE Trains
(
	Id INT NOT NULL PRIMARY KEY IDENTITY,
	HourOfDeparture VARCHAR(5) NOT NULL,
	HourOfArrival VARCHAR(5) NOT NULL,
	DepartureTownId INT NOT NULL FOREIGN KEY REFERENCES Towns(Id),
	ArrivalTownId INT NOT NULL FOREIGN KEY REFERENCES Towns(Id)
)

CREATE TABLE TrainsRailwayStations
(
	TrainId INT NOT NULL FOREIGN KEY REFERENCES Trains(Id),
	RailwayStationId INT NOT NULL FOREIGN KEY REFERENCES RailwayStations(Id),
	--Composite PRIMARY KEY
	CONSTRAINT PK_TrainsRailwayStations PRIMARY KEY(TrainId,RailwayStationId)
)

CREATE TABLE MaintenanceRecords
(
	Id INT NOT NULL PRIMARY KEY IDENTITY,
	DateOfMaintenance DATE NOT NULL,
	Details VARCHAR(2000) NOT NULL,
	TrainId INT NOT NULL FOREIGN KEY REFERENCES Trains(Id)
)

CREATE TABLE Tickets
(
	Id INT NOT NULL PRIMARY KEY IDENTITY,
	Price DECIMAL (10,2) NOT NULL,
	DateOfDeparture DATE NOT NULL,
	DateOfArrival DATE NOT NULL,
	TrainId INT NOT NULL FOREIGN KEY REFERENCES Trains(Id),
	PassengerId INT NOT NULL FOREIGN KEY REFERENCES Passengers(Id)
)
--2.Insert

INSERT INTO Trains(HourOfDeparture,HourOfArrival,DepartureTownId,ArrivalTownId)
VALUES ('07:00', '19:00', 1, 3),
	   ('08:30', '20:30', 5,  6),
	   ('09:00', '21:00', 4, 8),
	   ('06:45', '03:55', 27, 7),
	   ('10:15', '12:15', 15, 5)


INSERT INTO TrainsRailwayStations(TrainId,RailwayStationId)
VALUES (36, 1),
       (36, 4),
	   (36, 31),
	   (36, 57),
	   (36, 7),
	   (37, 13),
	   (37, 54),
	   (37, 60),
	   (37, 16),
	   (38, 10),
	   (38, 50),
	   (38, 52),
	   (38, 22),
	   (39, 68),
	   (39, 3),
	   (39, 31),
	   (39, 19),
	   (40, 41),
	   (40, 7),
	   (40, 52),
	   (40, 13)

INSERT INTO Tickets(Price,DateOfDeparture,DateOfArrival,TrainId,PassengerId)
VALUES (90.00, '2023-12-01', '2023-12-01', 36, 1),
       (115.00, '2023-08-02', '2023-08-02', 37, 2),
	   (160.00, '2023-08-03', '2023-08-03', 38, 3),
	   (255.00, '2023-09-01', '2023-09-02', 39, 21),
	   (95.00, '2023-09-02', '2023-09-03', 40, 22)

--3.Update

UPDATE Tickets
   SET DateOfArrival = DATEADD(DAY,7,DateOfArrival)
 WHERE DateOfDeparture > '2023-10-31'

 UPDATE Tickets
   SET DateOfDeparture = DATEADD(DAY,7,DateOfDeparture)
 WHERE DateOfDeparture > '2023-10-31'
--4. Delete

BEGIN TRANSACTION
DECLARE  @trainsToDelete TABLE(Id INT)

INSERT INTO @trainsToDelete (Id)
SELECT Id FROM Trains
WHERE DepartureTownId = 3

DELETE FROM Tickets
WHERE TrainId IN(SELECT Id FROM @trainsToDelete)

DELETE FROM MaintenanceRecords
WHERE TrainId IN(SELECT Id FROM @trainsToDelete)

DELETE FROM TrainsRailwayStations
WHERE TrainId IN(SELECT Id FROM @trainsToDelete)

DELETE FROM Trains
WHERE [Id] IN(SELECT Id FROM @trainsToDelete)

COMMIT

--5.Tickets by Price and Date of Departure

SELECT 
	   DateOfDeparture,
	   Price AS TicketPrice
  FROM Tickets
ORDER BY Price, DateOfDeparture DESC

--6.Passengers with their Tickets

SELECT 
       p.[Name],
	   t.Price,
	   t.DateOfDeparture,
	   t.TrainId
  FROM Passengers AS p
  JOIN Tickets AS t ON p.Id = t.PassengerId
 GROUP BY t.Price,p.[Name],t.DateOfDeparture,t.TrainId
 ORDER BY t.Price DESC


--7.Railway Stations without Passing Trains

SELECT 
	    t.[Name] AS Town,
	    r.[Name] AS RailwayStation
  FROM  Towns AS t
  LEFT  JOIN RailwayStations AS r ON t.Id = r.TownId
  LEFT  JOIN TrainsRailwayStations AS trs ON r.Id = trs.RailwayStationId
  LEFT  JOIN Trains AS tr ON trs.TrainId = tr.Id
 WHERE  tr.Id IS NULL
 ORDER  BY t.[Name],r.[Name]

--8.First 3 Trains Between 8:00 and 8:59 

SELECT TOP 3
	   t.Id AS TrainId,  
	   t.HourOfDeparture,
	   ti.Price AS TicketPrice,
	   tow.[Name] AS Destination
  FROM Trains AS t
  JOIN Tickets AS ti ON t.Id = ti.TrainId
  JOIN Towns AS tow ON t.ArrivalTownId = tow.Id
 WHERE ti.Price > 50 AND CAST(t.HourOfDeparture AS TIME ) >= '08:00' AND CAST(t.HourOfDeparture AS TIME ) <= '08:59'
 ORDER BY ti.Price 

--9.Count of Passengers Paid More Than Average With Arrival Towns

SELECT 
       tow.[Name] AS TownName,
	   COUNT(p.Id) AS PassengersCount
  FROM Passengers AS p
  JOIN Tickets AS t ON p.Id = t.PassengerId
  JOIN Trains AS tr ON tr.Id = t.TrainId
  JOIN Towns AS tow ON tr.ArrivalTownId = tow.Id
 WHERE t.Price > 76.99
 GROUP BY tow.[Name]
 ORDER BY tow.[Name]


--10.Maintenance Inspection with Town

	SELECT 
		   tr.Id,
		   tow.[Name],
		   m.Details
	  FROM Trains AS tr
	  JOIN Towns AS tow ON tr.DepartureTownId = tow.Id
	  JOIN MaintenanceRecords AS m ON tr.Id = m.TrainId
	 WHERE m.Details LIKE '%inspection%'
  ORDER BY tr.Id

--11.Towns with Trains

CREATE FUNCTION udf_TownsWithTrains(@name VARCHAR(30)) 
RETURNS INT
AS
BEGIN 
  RETURN  
  (
	SELECT 
		   COUNT(t.Id) AS TrainCount
	  FROM Trains AS t
	  JOIN Towns AS tow ON t.ArrivalTownId = tow.Id
	  JOIN Towns AS tow2 ON t.DepartureTownId = tow2.Id
	 WHERE tow.[Name] = @name OR tow2.[Name] = @name
 )
END

SELECT dbo.udf_TownsWithTrains('Paris')


--12.Search Passenger Travelling to Specific Town

CREATE PROCEDURE usp_SearchByTown(@townName VARCHAR(30)) 
AS
BEGIN
SELECT 
	   p.[Name],
	   t.DateOfDeparture,
	   tr.HourOfDeparture
  FROM Passengers AS p
  JOIN Tickets AS t ON p.Id = t.PassengerId
  JOIN Trains AS tr ON t.TrainId = tr.Id
  JOIN Towns AS tow ON tr.ArrivalTownId = tow.Id
 WHERE tow.[Name] = @townName
 ORDER BY t.DateOfDeparture DESC, p.[Name]
END

EXEC usp_SearchByTown 'Berlin'

SELECT * FROM Passengers
SELECT * FROM Towns
SELECT * FROM Trains
SELECT * FROM Tickets
SELECT * FROM RailwayStations
SELECT * FROM TrainsRailwayStations
SELECT * FROM MaintenanceRecords
