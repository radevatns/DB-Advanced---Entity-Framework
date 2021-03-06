--CREATE DATABASE MinionsDB
--USE MinionsDB

CREATE TABLE Countries(
Id INT IDENTITY PRIMARY KEY,
Name NVARCHAR(30) NOT NULL
)

CREATE TABLE Towns(
Id INT IDENTITY PRIMARY KEY,
Name NVARCHAR(30) NOT NULL,
CountryId INT NOT NULL,
CONSTRAINT FK_Towns_Counties FOREIGN KEY (CountryId)
	REFERENCES Countries (Id)
)

CREATE TABLE Minions(
Id INT IDENTITY PRIMARY KEY,
Name NVARCHAR(30) NOT NULL,
Age INT,
TownId INT NOT NULL,
CONSTRAINT FK_Minions_Towns FOREIGN KEY (TownId)
	REFERENCES Towns (Id)
)

CREATE TABLE Villains(
Id INT IDENTITY PRIMARY KEY,
Name NVARCHAR(30) NOT NULL,
Evilness NVARCHAR(20)
	CHECK (Evilness IN ('good', 'bad', 'evil', 'super evil'))
)

CREATE TABLE MinionsVillains(
MinionId INT,
VillainId INT,
CONSTRAINT PK_MinionsVillains 
	PRIMARY KEY (MinionId, VillainId),
CONSTRAINT FK_MinionsVillains_Minions FOREIGN KEY (MinionId)
	REFERENCES Minions(Id),
CONSTRAINT FK_MinionsVillains_Villains FOREIGN KEY (VillainId)
	REFERENCES Villains(Id)
)

INSERT INTO Countries (Name)
VALUES ('Bulgaria'), ('Englang'), ('Norway'), ('Germany'), ('France')

INSERT INTO Towns (Name, CountryId)
VALUES 
	('Sofia', 1),
	('London', 2),
	('Oslo', 3),
	('Berlin', 4),
	('Paris', 5)

INSERT INTO Minions (Name, Age, TownId)
VALUES
	('Dave', 15, 1),
	('Mark', 20, 2),
	('Jerry', 25, 3),
	('Jorge', 30, 4),
	('Tim', 35, 5)

INSERT INTO Villains (Name, Evilness)
VALUES
	('Gru', 'bad'),
	('Viktor', 'good'),
	('Jilly', 'evil'),
	('Poppy', 'super evil'),
	('Jimmy', 'good')

INSERT INTO MinionsVillains (MinionId, VillainId)
VALUES	
	(1, 2),
	(2, 3),
	(5, 2), 
	(3, 3), 
	(4, 3),
	(2, 2),
	(1, 1), 
	(4, 2),
	(1, 3)