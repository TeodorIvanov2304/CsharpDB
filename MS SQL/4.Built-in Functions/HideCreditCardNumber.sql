--Hide credit card number
SELECT [CustomerID],
       [FirstName],
	   [LastName],
	   CONCAT(LEFT([PaymentNumber],6),'*********')
    AS [PaymentNumber]
FROM Customers

--Create VIEW
CREATE VIEW v_Public_Customer_Info AS SELECT 
	   [CustomerID],
       [FirstName],
	   [LastName],
	   CONCAT(LEFT([PaymentNumber],6),'*********')
    AS [PaymentNumber]
FROM Customers

--Use the view
SELECT * FROM [dbo].[v_Public_Customer_Info]