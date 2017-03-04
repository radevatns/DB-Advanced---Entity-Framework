USE MinionsDB
EXEC usp_GetOlder @mid

SELECT Name, Age
FROM Minions
WHERE Id = @mid