CREATE DATABASE [Movies]

USE [Movies]

CREATE TABLE [Directors]
(
	[Id] INT PRIMARY KEY IDENTITY(1,1),
	[DirectorName] NVARCHAR(100) NOT NULL,
	[Notes] NVARCHAR(MAX)
)

INSERT INTO Directors ([DirectorName])
     VALUES ('Steven Spielberg'),
			('Alfred Hitchcock'),
			('Francis Ford Coppola'),
			('Martin Scorsese'),
			('Stanley Kubrick')

CREATE TABLE [Genres]
(
	[Id] INT PRIMARY KEY IDENTITY(1,1),
	[GenreName] NVARCHAR(100) NOT NULL,
	[Notes] NVARCHAR(MAX)
)

INSERT INTO Genres ([GenreName])
	 VALUES ('Drama'),
			('Action'),
			('Sci-fi'),
			('Horror'),
			('Mystery')

CREATE TABLE [Categories]
(
	 [Id] INT PRIMARY KEY IDENTITY(2,2),
	 [CategoryName] NVARCHAR(100) NOT NULL,
	 [Notes] NVARCHAR(MAX)
)

INSERT INTO Categories ([CategoryName])
	 VALUES ('Romance'),
			('Western'),
			('Comedy'),
			('Animation'),
			('Documentary')

CREATE TABLE [Movies]
(
	[Id] INT PRIMARY KEY IDENTITY(1,1),
	[Title] NVARCHAR(300)NOT NULL,
	[DirectorId] INT FOREIGN KEY REFERENCES Directors(Id),
	[CopyrightYear] INT NOT NULL,
	[Length] VARCHAR(50) NOT NULL,
	[GenreId] INT FOREIGN KEY REFERENCES Genres(Id),
	[CategoryId] INT FOREIGN KEY REFERENCES Categories(Id),
	[Rating] TINYINT,
	[Notes] VARCHAR(MAX)
)


INSERT INTO Movies([Title],[CopyrightYear],[Length])
	 VALUES ('The Shining',1980,'02:26:01'),
			('The Dark Knight',2008,'02:32:00'),
			('The Empire Strikes Back',1980,'02:04:09'),
			('The Matrix',1999,'02:16:21'),
			('Inception',2010,'02:28:12')

