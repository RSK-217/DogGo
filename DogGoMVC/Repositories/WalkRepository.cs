using DogGoMVC.Models;
using DogGoMVC.Helpers;
using DogGoMVC.Interfaces;
using Microsoft.Data.SqlClient;


namespace DogGoMVC.Repositories
{
    public class WalkRepository : BaseRepository, IWalkRepository
    {
        public WalkRepository(IConfiguration config) : base(config) { }

        private readonly string _baseSqlSelect = @"SELECT Walks.Id,
                                    	   [Date],
                                    	   Duration,
                                    	   WalkerId,
                                           Walker.[Name] AS WalkerName, 
                                    	   DogId,
                                           Dog.[Name] AS DogName 
                                    FROM Walks
                                    INNER JOIN Dog ON Dog.Id = Walks.DogId
                                    INNER JOIN Walker ON Walker.Id = Walks.WalkerId";
                                    
        public List<Walk> GetAllWalks()
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = _baseSqlSelect;

                    using (var reader = cmd.ExecuteReader())
                    {
                        var results = new List<Walk>();
                        
                        while (reader.Read())
                        {
                            
                            var walk = LoadFromData(reader);
                            walk.Duration = Helpers.Helpers.DurationFromSecondsToMinutes(walk.Duration);
                            results.Add(walk);
                        }

                        return results;
                    }
                }
            }
        }
        public List<Walk> GetWalksByWalker(int id)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT Walks.Id,
                                    	   [Date],
                                    	   Duration,
                                    	   WalkerId,
                                    	   DogId,
                                    	   [Owner].[Name] AS OwnerName
                                    FROM Walks
                                    INNER JOIN Dog ON Dog.Id = Walks.DogId
                                    INNER JOIN [Owner] ON [Owner].Id = Dog.OwnerId
                                    WHERE Walks.WalkerId = @Id";

                    cmd.Parameters.AddWithValue("@Id", id);

                    using (var reader = cmd.ExecuteReader())
                    {
                        List<Walk> results = new List<Walk>();
                        while (reader.Read())
                        {
                            Walk walk = LoadFromData(reader);
                            walk.Dog.Owner.Name = reader.GetString(reader.GetOrdinal("OwnerName"));
                            results.Add(walk);
                        }

                        return results;
                    }
                }
            }
        }

        public void CreateWalk(Walk walk)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"INSERT INTO Walks (
                                            [Date],
                                            Duration,
                                            WalkerId,
                                            DogId)
                                        VALUES (
                                            @Date,
                                            @Duration,
                                            @WalkerId,
                                            @DogId)";
                    cmd.Parameters.AddWithValue("@Date", walk.Date);
                    cmd.Parameters.AddWithValue("@Duration", Helpers.Helpers.DurationFromMinutesToSeconds(walk.Duration));
                    cmd.Parameters.AddWithValue("@WalkerId",walk.WalkerId);
                    cmd.Parameters.AddWithValue("@DogId",walk.Date);

                    cmd.ExecuteNonQuery();
                }
            }
        }
        private Walk LoadFromData(SqlDataReader reader)
        {
            return new Walk
            {
                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                Date = reader.GetDateTime(reader.GetOrdinal("Date")),
                Duration = reader.GetInt32(reader.GetOrdinal("Duration")),
                WalkerId = reader.GetInt32(reader.GetOrdinal("WalkerId")),
                DogId = reader.GetInt32(reader.GetOrdinal("DogId")),
                Dog = new Dog { Name = reader.GetString(reader.GetOrdinal("DogName")) },
                Walker = new Walker { Name = reader.GetString(reader.GetOrdinal("WalkerName")) }
            };
        }
    }
}
