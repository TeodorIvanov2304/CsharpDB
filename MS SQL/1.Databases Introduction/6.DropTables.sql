DROP TABLE [Towns]
DROP TABLE [Minions]

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

--Ctrl + Shift + R - to refresh the editor
--Ctrl + K,Ctrl + C - to comment the current selection
--Ctrl + K,Ctrl + U - to uncomment the current selection

--08. Create Table Users
-- VARBINARY holds memory used

CREATE TABLE [Users]
(
	[Id] BIGINT PRIMARY KEY IDENTITY,
	[Username] VARCHAR(30) NOT NULL,
	[Password] VARCHAR(26) NOT NULL,
	[ProfilePicture] VARBINARY(MAX),
	[LastLoginTime] DATETIME2,
	[IsDeleted] BIT
)

INSERT INTO [Users] ([Username],[Password])
	 VALUES ('Garo','1212ff'),
			('Zaro','31314s'),
			('Maro','asdad11'),
			('Sharo','fgfgg2'),
			('Karo','2131fg222')
			
     SELECT * FROM [Users]

--09. Change Primary Key (Composite Key from 2 values)
--First DELETE the primary key
ALTER TABLE [Users]
DROP CONSTRAINT PK__Users__3214EC07B4793025
--Second we add new Primary Key, combination from 2 values
ALTER TABLE [Users]
ADD CONSTRAINT PK_UsersTable PRIMARY KEY([Id],[Username])

--10.Check constraint for password field
ALTER TABLE [Users]
ADD CONSTRAINT CHK_PasswordIsAtLeastFiveSymbols
	--Check if lenght of the password is at least 5 symbols
	CHECK (LEN(Password) >= 5)