--09. Change Primary Key (Composite Key from 2 values)
--First DELETE the primary key
ALTER TABLE [Users]
DROP CONSTRAINT PK__Users__3214EC07B4793025
--Second we add new Primary Key, combination from 2 values
ALTER TABLE [Users]
ADD CONSTRAINT PK_UsersTable PRIMARY KEY([Id],[Username])