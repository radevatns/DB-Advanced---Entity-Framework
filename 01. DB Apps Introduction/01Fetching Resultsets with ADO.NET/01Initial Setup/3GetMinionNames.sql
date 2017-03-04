USE MinionsDB

SELECT Name FROM Villains
WHERE Id = @inputNum



--SELECT * FROM Minions AS m
--JOIN MinionsVillains AS mv
--ON mv.MinionId = m.Id
--JOIN Villains AS v
--ON v.Id = mv.VillainId
--WHERE mv.VillainId = @inputNum

--ORDER BY mv.VillainId
--SELECT * FROM MinionsVillains
--SELECT * FROM Villains