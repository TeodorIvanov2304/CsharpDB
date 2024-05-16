
USE [Minions]

--07. Create Table People

CREATE TABLE [People]
(
	[Id] INT PRIMARY KEY IDENTITY(1,1),
	[Name] NVARCHAR(200)NOT NULL,
	[Picture] VARBINARY(MAX),
	--3 is for maximal numbers in digit, 2 is for numbers after floating point
	[Height] DECIMAL(3,2),
	[Weight] DECIMAL(5,2),
	[Gender] CHAR(1)NOT NULL,
			 CHECK([Gender] in ('m','f')),
    [Birthdate] DATETIME2 NOT NULL,
	[Biography] NVARCHAR(MAX)
)

INSERT INTO People([Name],[Gender],[Birthdate])
	 VALUES ('Radka','f','1968.05.02'),
			('Pehso','m','1998.05.02'),
			('Ivan','m','1997.05.02'),
			('Petkan','m','1991.05.02'),
			('Dragan','m','1992.05.02')


SELECT * FROM People

--If we forgot to make Id PRIMARY KEY, execute the following query
ALTER TABLE People
ADD CONSTRAINT PK_People
PRIMARY KEY (Id)

