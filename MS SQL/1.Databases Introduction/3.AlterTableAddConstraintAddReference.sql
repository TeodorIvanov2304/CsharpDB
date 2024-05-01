ALTER TABLE Minions
/*
FK stands for Foreign key. We add reference from FOREIGN KEY(TownId) to Towns(Id)
*/
ADD CONSTRAINT FK_Minions_Towns
FOREIGN KEY(TownId) REFERENCES Towns(Id)
