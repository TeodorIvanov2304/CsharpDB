USE Softuni

--Back up database
BACKUP DATABASE Softuni
TO DISK = 'D:\IMPORTANT PROGRAMS\SQL Backup\softuni-backup.bak'

--Restore database
USE master;
GO
RESTORE DATABASE SoftUni 
FROM DISK = 'D:\IMPORTANT PROGRAMS\SQL Backup\softuni-backup.bak';