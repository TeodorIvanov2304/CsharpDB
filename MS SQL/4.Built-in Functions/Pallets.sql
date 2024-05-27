--Calculate the required number of pallets to ship each item
--BoxCapacity specifies how many items can fit in one box
--PalletCapacity specifies how many boxes can fit in a pallet

SELECT 
  CEILING
	(CEILING
		(CAST([Quantity] AS Float) / [BoxCapacity]) / [PalletCapacity])
		   AS [Number of Pallets]
         FROM [Products] 