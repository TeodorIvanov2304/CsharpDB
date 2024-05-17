CREATE DATABASE CarRental
USE CarRental

CREATE TABLE Categories
(
	[Id] INT PRIMARY KEY IDENTITY,
	[CategoryName] NVARCHAR(100) NOT NULL,
	[DailyRate] DECIMAL(10,2)NOT NULL,
	[WeeklyRate] DECIMAL(10,2)NOT NULL,
	[MonthlyRate] DECIMAL(10,2)NOT NULL,
	[WeekendRate] DECIMAL(10,2) NOT NULL
)

INSERT INTO Categories(CategoryName,DailyRate,WeeklyRate,MonthlyRate,WeekendRate)
     VALUES ('Truck',1.1,1.2,1.3,1.4),
			('Car',2.1,2.3,2.4,2.5),
			('SUV',3.1,3.2,3.3,3.4)

CREATE TABLE Cars
(
	[Id] INT PRIMARY KEY IDENTITY,
	[PlateNumber] NVARCHAR(10)NOT NULL,
	[Manufacturer] NVARCHAR(90) NOT NULL,
	[Model] NVARCHAR(90) NOT NULL,
	[CarYear] INT NOT NULL,
	[CategoryId] INT FOREIGN KEY REFERENCES Categories(Id) NOT NULL,
	[Doors] TINYINT NOT NULL,
	[Picture] IMAGE,
	[Condition] NVARCHAR(90) NOT NULL,
	[Available] CHAR(1)NOT NULL,
	--Available BIT DEFAULT 1
			    CHECK([Available] in ('y','n')),
)

INSERT INTO Cars(PlateNumber, Manufacturer, Model, CarYear, CategoryId, Doors,Condition,Available)
	 VALUES ('CB1112','Toyota','Corolla',2005,1,4,'Good','y'),
			('CB2142','Subaru','Forester',2005,2,5,'Bad','y'),
			('PB4412','BMW','X6',2020,3,5,'Good','y')

CREATE TABLE Employees
(
	[Id] INT PRIMARY KEY IDENTITY(1,1),
	[FirstName] NVARCHAR(100) NOT NULL,
	[LastName] NVARCHAR(100) NOT NULL,
	[Title] NVARCHAR(50) NOT NULL,
	[Notes] NVARCHAR(MAX)
)

INSERT INTO Employees(FirstName,LastName,Title)
	 VALUES ('Joro','Kamenov','Driver'),
			('Yasen','Petrov','Sales Manager'),
			('Georgi','Todorov','Mechanic')

CREATE TABLE Customers 
(
	[Id] INT PRIMARY KEY IDENTITY(1,1),
	[DriverLicenceNumber] NVARCHAR(20) UNIQUE NOT NULL,
	[FullName] NVARCHAR(200) NOT NULL,
	[Address] NVARCHAR(250) NOT NULL,
	[City] NVARCHAR(100) NOT NULL,
	[ZIPCode] INT NOT NULL,
	[Notes] NVARCHAR(MAX)
)

INSERT INTO Customers (DriverLicenceNumber,FullName,[Address],City,ZIPCode)
     VALUES ('21345692019234156921','Vin Diesel','Beverly Hills','Los Angeles',90001),
			('11346662019234156921','Selski Bek','Mladost-4','Sofia',1715),
			('91346662019999156921','Mika Hakinen','Kallio','Helsinki',00500)

CREATE TABLE RentalOrders (
	Id INT PRIMARY KEY NOT NULL, 
	EmployeeId INT NOT NULL FOREIGN KEY REFERENCES Employees(Id), 
	CustomerId INT NOT NULL FOREIGN KEY REFERENCES Customers(Id), 
	CarId INT NOT NULL FOREIGN KEY REFERENCES Cars(Id), 
	TankLevel DECIMAL(5,2), 
	KilometrageStart INT, 
	KilometrageEnd INT, 
	TotalKilometrage INT, 
	StartDate DATE NOT NULL, 
	EndDate DATE NOT NULL, 
	TotalDays INT NOT NULL, 
	RateApplied DECIMAL(10, 2), 
	TaxRate DECIMAL(10, 2), 
	OrderStatus NVARCHAR(50), 
	NOTES NVARCHAR(MAX)
)

INSERT INTO RentalOrders (Id, EmployeeId, CustomerId, CarId, StartDate, EndDate, TotalDays) VALUES 
(1, 3, 3, 3, '01-01-2020', '01-02-2020', 1), 
(2, 1, 1, 1, '01-01-2020', '01-03-2020', 2),
(3, 2, 2, 2, '01-01-2020', '01-04-2020', 3)