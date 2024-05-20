USE [Geography]

SELECT * 
  FROM  [Peaks]

--Crateing a simple view
--VIEWS allways starts and ends with GO!
--We add OR ALTER if we want to change the already saved VIEW

GO
CREATE OR ALTER VIEW [v_HigestPeak] AS
									(SELECT TOP(5) *, 
											GETDATE()
									AS  [Execution DateTime]
									FROM  [Peaks]
									ORDER BY [Elevation] DESC)

GO

--Using the view. Refresh and see Views for the name
SELECT *
  FROM [dbo].[v_HigestPeak]