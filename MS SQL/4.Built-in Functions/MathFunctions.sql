USE [Demo]

--Area of a triangle
SELECT [Id],[A],[H], (A*H)/2
    AS Area
  FROM Triangles2


--Pi
SELECT PI()


--Absolut value
SELECT ABS(-5)

--Square root
SELECT SQRT(9)

--Square
SELECT SQUARE(5)

--Power
SELECT POWER(2,3)

--Round
SELECT ROUND(2.88887654,2)

--Floor & Celing
SELECT FLOOR(2.567)
SELECT CEILING(2.567)

--Sign
SELECT SIGN(-100)

--Rand
--Default float numbers between 0 and 1
SELECT RAND()

--Random numbers between 1 and 100
SELECT FLOOR(RAND() * 100)
