--Simple JOIN

SELECT 
       p.PeakName,
	   m.MountainRange,
	   p.Elevation
  FROM [Peaks] AS p
  JOIN [Mountains] AS m ON p.MountainId = m.Id
  WHERE m.MountainRange = 'Rila'
  ORDER BY [Elevation] DESC


--Second solution with AND

  SELECT 
       p.PeakName,
	   m.MountainRange,
	   p.Elevation
  FROM [Peaks] AS p
  JOIN [Mountains] AS m ON p.MountainId = m.Id AND m.MountainRange = 'Rila' 
  ORDER BY [Elevation] DESC
