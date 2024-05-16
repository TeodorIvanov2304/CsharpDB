--Set DEFAULT VALUE OF FIELD
ALTER TABLE [Users]
  ADD CONSTRAINT DF_DefaultLoginTime DEFAULT (CURRENT_TIMESTAMP) FOR [LastLoginTime]

INSERT INTO [Users] ([Username],[Password],[ProfilePicture],[LastLoginTime],[IsDeleted])
     VALUES ('Taro','12121',NULL,DEFAULT,NULL)

SELECT * FROM [Users]