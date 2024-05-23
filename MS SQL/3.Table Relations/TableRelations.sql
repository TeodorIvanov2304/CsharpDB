--1.One-To-One Relationship

GO
CREATE TABLE [Passports]
(
	[PassportID] INT PRIMARY KEY IDENTITY(101,1),
	[PassportNumber] VARCHAR(50) NOT NULL
)

CREATE TABLE [Persons]
(
	[PersonID] INT PRIMARY KEY IDENTITY(1,1),
	[FirstName] VARCHAR(50) NOT NULL,
	[Salary] DECIMAL(10,2),
	[PassportID] INT UNIQUE FOREIGN KEY REFERENCES Passports(PassportID)
)

INSERT INTO [Passports]
     VALUES ('N34FG21B'),
			('K65LO4R7'),
			('ZE657QP2')

INSERT INTO [Persons]
	 VALUES ('Roberto',43300,102),
			('Tom',56100,103),
			('Yana',60200,101)

SELECT * FROM [Persons]

GO

--2.One-To-Many Relationship

CREATE TABLE Manufacturers
(
	[ManufacturerID] INT PRIMARY KEY IDENTITY(1,1),
	[Name] VARCHAR(32) NOT NULL,
	[EstablishedOn] DATETIME2
)


--Not UNIQUE, because every manufacturer has more than 1 model
CREATE TABLE Models
(
	[ModelID] INT PRIMARY KEY IDENTITY(101,1),
	[Name] VARCHAR(32) NOT NULL,
	[ManufacturerID] INT FOREIGN KEY REFERENCES Manufacturers(ManufacturerID)
)

INSERT INTO [Manufacturers]
     VALUES ('BMW','07/03/1916'),
			('Tesla','01/01/2003'),
			('Lada','01/05/1966')

INSERT INTO [Models]
     VALUES ('X1',1),
			('i6',1),
			('Model S',2),
			('Model X',2),
			('Model 3',2),
			('Nova',3)

GO

--3.Many-To-Many Relationship
CREATE TABLE [Students]
(
	[StudentID] INT PRIMARY KEY IDENTITY(1,1),
	[Name] VARCHAR(50) NOT NULL
)

CREATE TABLE [Exams]
(
	[ExamID] INT PRIMARY KEY IDENTITY(101,1),
	[Name] VARCHAR(50) NOT NULL
)

CREATE TABLE [StudentsExams]
(
	[StudentID] INT FOREIGN KEY REFERENCES [Students](StudentID),
	[ExamID] INT FOREIGN KEY REFERENCES [Exams](ExamID),
	--CREATING COMPOSITE PRIMARY KEY
	CONSTRAINT PK_StudentsExams PRIMARY KEY (StudentID,ExamID)
)


INSERT INTO [Exams]
     VALUES ('SpringMVC'),
			('Neo4j'),
			('Oracle 11g')

INSERT INTO [Students]
     VALUES ('Mila'),
			('Toni'),
			('Ron')

INSERT INTO [StudentsExams]
     VALUES (1,101),
			(1,102),
			(2,101),
			(3,103),
			(2,102),
			(2,103)

GO

--JOIN 03. tables
SELECT * 
FROM StudentsExams
JOIN Students ON Students.StudentID = StudentsExams.StudentID
JOIN Exams ON Exams.ExamID = StudentsExams.ExamID


--04.Self-Referencing
CREATE TABLE [Teachers]
(	
	[TeacherID] INT PRIMARY KEY,
	[Name] VARCHAR(50) NOT NULL,
	--ManagerID is self-reference. It references TeacherID, so some of the teachers will be also managers
	--and will use the same ID
	[ManagerID] INT REFERENCES [Teachers](TeacherID)
)

INSERT INTO [Teachers]
	 VALUES (101,'John',NULL),
			(106,'Greta',101),
			(105,'Mark',101),
			(104,'Ted',105),
			(103,'Sivlia',106),
			(102,'Maya',106)

--05.Online Store Database

