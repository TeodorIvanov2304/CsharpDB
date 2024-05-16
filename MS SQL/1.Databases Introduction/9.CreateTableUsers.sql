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