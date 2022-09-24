SELECT Walker.Id,
       Walker.[Name],
       ImageUrl, 
       NeighborhoodId,
       Neighborhood.Name AS NeighborhoodName,
       Walks.[Date] AS WalkDate,
       Walks.DogId AS Dog,
       Walks.Duration AS Duration
       FROM Walker
       INNER JOIN Walks ON Walker.Id = WalkerId
       INNER JOIN Neighborhood ON Neighborhood.Id = NeighborhoodId
       

SELECT Duration
FROM Walks