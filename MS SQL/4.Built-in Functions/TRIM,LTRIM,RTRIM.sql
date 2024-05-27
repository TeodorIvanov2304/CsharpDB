
--Removes empty spaces left from the string 'Hi'
SELECT LTRIM('    Hi    ')

--Removes empty spaces right from the string 'Hi'
SELECT RTRIM('    Hi    ')

--Removes left and right empty spaces

SELECT TRIM('  Hi   ')

--Trims all given symbols, letters or numbers, in this case '-'
SELECT TRIM('-' FROM '--Hi--')

