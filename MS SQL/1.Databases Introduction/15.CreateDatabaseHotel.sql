CREATE DATABASE Hotel
USE Hotel
CREATE TABLE Employees
(
	[Id] INT PRIMARY KEY IDENTITY,
	[FirstName] NVARCHAR(50) NOT NULL,
	[LastName] NVARCHAR(50) NOT NULL,
	[Title] NVARCHAR(50) NOT NULL,
	[Notes] NVARCHAR(MAX)
)

INSERT INTO Employees (FirstName,LastName,Title)
	 VALUES ('Hristo','Kirilov','Driver'),
			('Ivan','Stefanov','Cleaner'),
			('Mihail','Ivanov','Cleaner')

CREATE TABLE Customers
(
	[AccountNumber] INT PRIMARY KEY IDENTITY,
	[FirstName] NVARCHAR(50) NOT NULL,
	[LastName] NVARCHAR(50) NOT NULL,
	[PhoneNumber] VARCHAR(50),
	[EmergencyName] NVARCHAR(50) NOT NULL,
	[EmergencyNumber] VARCHAR(50) NOT NULL,
	[Notes] NVARCHAR(MAX)
)

INSERT INTO Customers(FirstName,LastName,EmergencyName,EmergencyNumber)
	 VALUES ('Yanko','Georgiev','Ivan Ivanov','0881223333'),
			('Misho','Mihailov','Ivan Ivanov','0881223343'),
			('Nikola','Nikolov','Ivan Ivanov','0841223333')

CREATE TABLE RoomStatus
(
	[RoomStatus] NVARCHAR(50)PRIMARY KEY NOT NULL,
	[Notes] NVARCHAR(MAX)
)

INSERT INTO RoomStatus (RoomStatus)
	 VALUES ('Avaible'),
			('Not avaible'),
			('Preparing')

CREATE TABLE RoomTypes
(
	[RoomType] NVARCHAR(50) PRIMARY KEY NOT NULL,
	[Notes] NVARCHAR(MAX)
)

INSERT INTO RoomTypes(RoomType)
	 VALUES ('Single'),
			('Double'),
			('President Apartment')

CREATE TABLE BedTypes 
(
	[BedType] NVARCHAR(50) PRIMARY KEY NOT NULL,
	[Notes] NVARCHAR(MAX)
)

INSERT INTO BedTypes(BedType)
	 VALUES ('Single'),
			('Double'),
			('King size')

CREATE TABLE Rooms
(
	[RoomNumber] INT PRIMARY KEY NOT NULL,
	[RoomType] NVARCHAR(50) NOT NULL FOREIGN KEY REFERENCES RoomTypes(RoomType),
	[BedType] NVARCHAR(50) NOT NULL FOREIGN KEY REFERENCES BedTypes(BedType),
	[Rate] DECIMAL(10,2) NOT NULL,
	[RoomStatus] NVARCHAR(50) NOT NULL,
	[Notes] NVARCHAR(MAX)
)

INSERT INTO Rooms (RoomNumber,RoomType,BedType,Rate,RoomStatus)
     VALUES (1,'Double','Double',4.9,'Avaible'),
			(2,'Single','Single',4.3,'Avaible'),
			(3,'President Apartment','King size',4.5,'Avaible')

CREATE TABLE Payments 
(
	[Id] INT PRIMARY KEY IDENTITY,
	[EmployeeId] INT FOREIGN KEY REFERENCES Employees(Id),
	[PaymentDate] DATE NOT NULL,
	[AccountNumber] INT NOT NULL,
	[FirstDateOccupied] DATE NOT NULL,
	[LastDateOccupied] DATE NOT NULL,
	[TotalDays] INT NOT NULL,
	[AmountCharged] MONEY NOT NULL,
	[TaxRate] DECIMAL(10,2) NOT NULL,
	[TaxAmount] DECIMAL(10,2) NOT NULL,
	[PaymentTotal] MONEY NOT NULL,
	[Notes] NVARCHAR(MAX)
)

INSERT INTO Payments (EmployeeId,PaymentDate,AccountNumber,FirstDateOccupied,LastDateOccupied,TotalDays,AmountCharged,TaxRate,TaxAmount,PaymentTotal)
	 VALUES (1,'10-10-2024',1,'09-10-2024','10-10-2024',1,100,2,3,100),
			(2,'10-11-2024',2,'09-11-2024','10-11-2024',1,120,2,3,100),
			(3,'10-12-2024',3,'09-12-2024','10-12-2024',1,150,2,3,100)

CREATE TABLE Occupancies
(
	[Id] INT PRIMARY KEY IDENTITY,
	[EmployeeId] INT FOREIGN KEY REFERENCES Employees(Id),
	[DateOccupied] DATE NOT NULL,
	[AccountNumber] INT NOT NULL,
	[RoomNumber] INT NOT NULL,
	[RateApplied] DECIMAL(10,2) NOT NULL,
	[PhoneCharge] VARCHAR(50) NOT NULL,
	[Notes] NVARCHAR(MAX)
)

INSERT INTO Occupancies(EmployeeId,DateOccupied,AccountNumber,RoomNumber,RateApplied,PhoneCharge)
	 VALUES (1,'01-24-2024',1,1,2.4,0881223332),
			(2,'01-24-2024',2,2,2.5,0881223333),
			(3,'01-24-2024',3,3,2.6,0886223333)