USE MinionsDB

SELECT v.Name, COUNT(vm.MinionId) AS MinionsCount FROM Villains AS v
INNER JOIN MinionsVillains AS vm
ON vm.VillainId = v.Id
GROUP BY v.Name
HAVING COUNT(vm.MinionId)>3 
ORDER BY MinionsCount DESC