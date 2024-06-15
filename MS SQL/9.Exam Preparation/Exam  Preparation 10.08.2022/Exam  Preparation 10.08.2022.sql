CREATE DATABASE NationalTouristSitesOfBulgaria

--Section 1. DDL (30 pts)

USE NationalTouristSitesOfBulgaria

GO
CREATE TABLE Categories
(
	Id INT NOT NULL PRIMARY KEY IDENTITY,
	[Name] VARCHAR(50) NOT NULL
)

CREATE TABLE Locations
(
	Id INT NOT NULL PRIMARY KEY IDENTITY,
	[Name] VARCHAR(50) NOT NULL,
	Municipality VARCHAR(50),
	Province VARCHAR(50)
)

CREATE TABLE Sites
(
	Id INT NOT NULL PRIMARY KEY IDENTITY,
	[Name] VARCHAR(100) NOT NULL,
	LocationId INT NOT NULL FOREIGN KEY REFERENCES Locations(Id),
	CategoryId INT NOT NULL FOREIGN KEY REFERENCES Categories(Id),
	Establishment VARCHAR(15)
)

CREATE TABLE Tourists
(
	Id INT NOT NULL PRIMARY KEY IDENTITY,
	[Name] VARCHAR(50) NOT NULL,
	Age INT NOT NULL
		CHECK(Age BETWEEN 0 AND 120),
	PhoneNumber VARCHAR(20) NOT NULL,
	Nationality VARCHAR(30) NOT NULL,
	Reward VARCHAR(20)
)

CREATE TABLE SitesTourists
(
	TouristId INT NOT NULL FOREIGN KEY REFERENCES Tourists(Id),
	SiteId INT NOT NULL FOREIGN KEY REFERENCES Sites(Id)
	CONSTRAINT PK_SitesTourists PRIMARY KEY(TouristId,SiteId)
)

CREATE TABLE BonusPrizes
(
	Id INT NOT NULL PRIMARY KEY IDENTITY,
	[Name] VARCHAR(50)  NOT NULL
)

CREATE TABLE TouristsBonusPrizes
(
	TouristId INT NOT NULL FOREIGN KEY REFERENCES Tourists(Id),
	BonusPrizeId INT NOT NULL FOREIGN KEY REFERENCES BonusPrizes(Id)
	CONSTRAINT PK_TouristsBonusPrizes PRIMARY KEY(TouristId,BonusPrizeId)
)

GO

--Section 2. DML (10 pts)

--2.Insert

INSERT INTO Tourists([Name],Age,PhoneNumber,Nationality,Reward)
	 VALUES ('Borislava Kazakova',	52,	'+359896354244', 'Bulgaria',NULL),
			('Peter Bosh',	48,'+447911844141','UK',NULL),
			('Martin Smith',29,'+353863818592','Ireland','Bronze badge'),
			('Svilen Dobrev',49,'+359986584786','Bulgaria','Silver badge'),
			('Kremena Popova',38,'+359893298604','Bulgaria',NULL)

INSERT INTO Sites([Name],LocationId,CategoryId,Establishment)
	 VALUES('Ustra fortress',90, 7,'X'),
	       ('Karlanovo Pyramids',65,7,NULL),
		   ('The Tomb of Tsar Sevt',63,8,'V BC'),
		   ('Sinite Kamani Natural Park',17,1,NULL),
		   ('St. Petka of Bulgaria – Rupite',92,6,'1994')

--3.Update

UPDATE Sites
   SET Establishment = '(not defined)'
 WHERE Establishment IS NULL

--4.Delete

BEGIN TRANSACTION

DELETE FROM TouristsBonusPrizes
WHERE BonusPrizeId = 5

DELETE FROM BonusPrizes
WHERE Id = 5

COMMIT

--5.Tourists

SELECT 
	   [Name],
	   Age,
	   PhoneNumber,
	   Nationality
  FROM Tourists
 ORDER BY Nationality,Age DESC,[Name]

--6.Sites with Their Location and Category

SELECT 
       s.[Name] AS [Site],
	   l.[Name] AS [Location],
	   s.Establishment,
	   c.[Name] AS Category
  FROM Sites AS s
  JOIN Locations AS l ON s.LocationId = l.Id
  JOIN Categories AS c ON s.CategoryId = c.Id
  ORDER BY Category DESC,[Location],s.[Name]

--7.Count of Sites in Sofia Province

SELECT 
	  l.Province,
	  l.Municipality,
	  l.[Name] AS [Location],
	  COUNT(s.Id) AS CountOfSites
 FROM Locations AS l
 JOIN Sites AS s ON l.Id = s.LocationId
 WHERE Province = 'Sofia'
 GROUP BY Province,Municipality,l.[Name]
 ORDER BY COUNT(s.Id) DESC,[Location]


--8.Tourist Sites established BC

SELECT 
       s.[Name] AS [Site],
	   l.[Name] AS [Location],
	   l.Municipality,
	   l.Province,
	   s.Establishment
  FROM Sites AS s
  JOIN Locations AS l ON s.LocationId = l.Id
  WHERE l.[Name] NOT IN('B%','D%','M%') AND s.Establishment LIKE '%_BC' AND l.Id != 43
  ORDER BY s.[Name]

--9.Tourists with their Bonus Prizes

SELECT 
       t.[Name],
	   t.Age,
	   t.PhoneNumber,
	   t.Nationality,
	   CASE 
			WHEN bp.[Name] IS NULL THEN  '(no bonus prize)'
			ELSE bp.[Name]
	   END  
	    AS Reward
  FROM Tourists AS t
LEFT  JOIN TouristsBonusPrizes AS tbp ON t.Id = tbp.TouristId
LEFT  JOIN BonusPrizes AS bp ON tbp.BonusPrizeId = bp.Id
ORDER BY t.[Name]

--10.Tourists visiting History and Archaeology sites

SELECT DISTINCT
       SUBSTRING(t.[Name] ,CHARINDEX(' ',t.[Name] ),LEN(t.[Name] ) - LEN(CHARINDEX(' ',t.[Name] ))) AS LastName, 
	   t.Nationality,
	   t.Age,
	   t.PhoneNumber
  FROM Tourists AS t
  JOIN SitesTourists AS st ON t.Id = st.TouristId
  JOIN Sites AS s ON st.SiteId = s.Id
  JOIN Categories AS c ON s.CategoryId = c.Id
 WHERE c.[Name] = 'History and archaeology'
 ORDER BY SUBSTRING(t.[Name] ,CHARINDEX(' ',t.[Name] ),LEN(t.[Name] ) - LEN(CHARINDEX(' ',t.[Name] )))
 

--11.Tourists Count on a Tourist Site

CREATE FUNCTION udf_GetTouristsCountOnATouristSite (@Site VARCHAR(100))
RETURNS INT
AS
BEGIN

DECLARE @touristCount INT = 
(
	SELECT 
		   COUNT(t.Id) 
	  FROM Sites AS s
	  JOIN SitesTourists AS st ON s.Id = st.SiteId
	  JOIN Tourists AS t ON st.TouristId = t.Id
	 WHERE s.[Name] = @Site
 )
RETURN @touristCount
END

SELECT dbo.udf_GetTouristsCountOnATouristSite ('Gorge of Erma River')


--12.Annual Reward Lottery

CREATE PROCEDURE usp_AnnualRewardLottery(@TouristName VARCHAR(50))
AS
BEGIN 
SELECT 
	   t.[Name],
       CASE
			WHEN COUNT(t.Id) >= 100 THEN 'Gold badge'
			WHEN COUNT(t.Id) < 100 AND COUNT(t.Id) >= 50 THEN 'Silver badge'
			WHEN COUNT(t.Id) < 50 AND COUNT(t.Id) >= 25 THEN 'Brozne badge'
		END AS Reward
  FROM Tourists AS t
  JOIN SitesTourists AS st ON t.Id = st.TouristId
  JOIN Sites AS s ON st.SiteId = s.Id
 WHERE t.[Name] = @TouristName
 GROUP BY t.[Name]
END

EXEC usp_AnnualRewardLottery 'Zac Walsh'
