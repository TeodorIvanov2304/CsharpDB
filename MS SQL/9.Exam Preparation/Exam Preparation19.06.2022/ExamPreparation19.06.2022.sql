CREATE DATABASE Zoo

--Section 1. DDL (30 pts)

USE Zoo

CREATE TABLE Owners
(
	Id INT PRIMARY KEY IDENTITY,
	[Name] VARCHAR(50) NOT NULL,
	PhoneNumber VARCHAR(15) NOT NULL,
	[Address] VARCHAR(50)
)

CREATE TABLE AnimalTypes
(
	Id INT PRIMARY KEY IDENTITY,
	AnimalType VARCHAR(30) NOT NULL
)

CREATE TABLE Cages
(
	Id INT PRIMARY KEY IDENTITY,
	AnimalTypeId INT NOT NULL FOREIGN KEY REFERENCES AnimalTypes(Id)
)

CREATE TABLE Animals
(
	Id INT PRIMARY KEY IDENTITY,
	[Name] VARCHAR(30) NOT NULL,
	BirthDate DATE NOT NULL,
	OwnerId INT FOREIGN KEY REFERENCES Owners(Id),
	AnimalTypeId INT NOT NULL FOREIGN KEY REFERENCES AnimalTypes(Id)
)

CREATE TABLE AnimalsCages
(
	CageId INT NOT NULL FOREIGN KEY REFERENCES Cages(Id),
	AnimalId INT NOT NULL FOREIGN KEY REFERENCES Animals(Id)
	CONSTRAINT PK_AnimalsCages PRIMARY KEY(CageId,AnimalId)
)

CREATE TABLE VolunteersDepartments
(
	Id INT PRIMARY KEY IDENTITY,
	DepartmentName VARCHAR(30) NOT NULL
)

CREATE TABLE Volunteers
(
	Id INT PRIMARY KEY IDENTITY,
	[Name] VARCHAR(50) NOT NULL,
	PhoneNumber VARCHAR(15) NOT NULL,
	[Address] VARCHAR(50),
	AnimalId INT FOREIGN KEY REFERENCES Animals(Id),
	DepartmentId INT NOT NULL FOREIGN KEY REFERENCES VolunteersDepartments(Id)
)

--Section 2. DML (10 pts)

--2.Insert

INSERT INTO Volunteers([Name],PhoneNumber,[Address],AnimalId,DepartmentId)
	 VALUES ('Anita Kostova', '0896365412',	'Sofia, 5 Rosa str.', 15, 1),
	        ('Dimitur Stoev', '0877564223',	NULL, 42, 4),
			('Kalina Evtimova', '0896321112',	'Silistra, 21 Breza str.', 9, 7),
			('Stoyan Tomov', '0898564100',	'Montana, 1 Bor str.', 18, 8),
			('Boryana Mileva', '0888112233', NULL, 31, 5)

INSERT INTO Animals([Name],BirthDate,OwnerId,AnimalTypeId)
	 VALUES ('Giraffe',	'2018-09-21', 21, 1),
	        ('Harpy Eagle',	'2015-04-17', 15, 3),
			('Hamadryas Baboon', '2017-11-02', NULL, 1),
			('Tuatara', '2021-06-30', 2, 4)

--3.Update

UPDATE Animals
   SET OwnerId = 4
 WHERE OwnerId IS NULL

--4.Delete

DELETE FROM Volunteers
WHERE DepartmentId = 2

DELETE FROM VolunteersDepartments
WHERE Id = 2

--5.Volunteers

SELECT 
       [Name],
	   PhoneNumber,
	   [Address],
	   AnimalId,
	   DepartmentId
  FROM Volunteers
 ORDER BY [Name],AnimalId,DepartmentId DESC


--6.Animals data

SELECT 
       a.[Name],
	   ant.AnimalType,
	   FORMAT(a.BirthDate,'dd.MM.yyyy') AS BirthDate
  FROM Animals AS a
  JOIN AnimalTypes AS ant ON a.AnimalTypeId = ant.Id
 ORDER BY a.[Name]

--7.Owners and Their Animals

SELECT TOP 5
       o.[Name] AS [Owner],
	   COUNT(a.OwnerId) AS CountOfAnimals
  FROM Owners AS o
  JOIN Animals AS a ON o.Id = a.OwnerId
 GROUP BY o.[Name]
 ORDER BY COUNT(a.OwnerId) DESC,o.[Name]


--8.Owners, Animals and Cages

SELECT 
       CONCAT(o.[Name],'-',a.[Name]) AS OwnersAnimals,
	   o.PhoneNumber,
	   ac.CageId
  FROM Owners AS o
  JOIN Animals AS a ON o.Id = a.OwnerId
  JOIN AnimalsCages AS ac ON a.Id = ac.AnimalId
  JOIN AnimalTypes AS aty ON a.AnimalTypeId = aty.Id
  WHERE aty.AnimalType = 'Mammals'
ORDER BY o.[Name],a.[Name] DESC

--9.Volunteers in Sofia

SELECT 
      v.[Name],
	  v.PhoneNumber,
	  LTRIM(SUBSTRING(LTRIM(v.[Address]),8,LEN(v.[Address])-5)) AS [Address]
 FROM Volunteers AS v
 JOIN VolunteersDepartments AS vd ON v.DepartmentId = vd.Id
 WHERE vd.DepartmentName = 'Education program assistant' AND v.[Address] LIKE '%Sofia%'
 ORDER BY v.[Name]

--10.Animals for Adoption

--When Table is not needed i get Invalid object name 'Owners'

	SELECT 
		   a.[Name],
		   DATEPART(YEAR,a.BirthDate) AS BirthYear,
		   aty.AnimalType
	  FROM Animals AS a
	  JOIN AnimalTypes AS aty ON a.AnimalTypeId = aty.Id
	 WHERE a.OwnerId IS NULL AND DATEDIFF(YEAR,a.BirthDate,'01/01/2022') < 5 AND aty.AnimalType NOT LIKE 'Birds'
	 ORDER BY a.[Name]

--11.All Volunteers in a Department

CREATE FUNCTION udf_GetVolunteersCountFromADepartment (@VolunteersDepartment VARCHAR(30)) 
RETURNS INT
AS 
BEGIN
DECLARE  @volunteerCount INT
SET @volunteerCount =
(
SELECT 
       COUNT(v.DepartmentId) AS [Count]
  FROM Volunteers AS v
  JOIN VolunteersDepartments AS vd ON v.DepartmentId = vd.Id
 WHERE vd.DepartmentName = @VolunteersDepartment
)
RETURN @volunteerCount
END

SELECT dbo.udf_GetVolunteersCountFromADepartment ('Guest engagement')

--12.Animals with Owner or Not

CREATE PROCEDURE usp_AnimalsWithOwnersOrNot(@AnimalName VARCHAR(30)) 
AS
BEGIN
SELECT 
       a.[Name],
	   CASE
			WHEN o.[Name] IS NULL THEN 'For adoption'
			WHEN o.[Name] IS NOT NULL THEN o.[Name] 
	    END AS OwnersName
  FROM Animals AS a
  LEFT  JOIN Owners AS o ON a.OwnerId = o.Id
 WHERE a.[Name] = @AnimalName
 END

EXEC usp_AnimalsWithOwnersOrNot 'Hippo'
