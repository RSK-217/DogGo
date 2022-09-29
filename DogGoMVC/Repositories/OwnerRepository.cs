using DogGoMVC.Interfaces;
using DogGoMVC.Models;
using Microsoft.Data.SqlClient;

namespace DogGoMVC.Repositories
{
    public class OwnerRepository : BaseRepository, IOwnerRepository
    {
        private readonly string _baseSqlSelect = @"SELECT [Owner].Id,
                                                     Email,
                                                     [Owner].[Name],
                                                     [Address],
                                                     NeighborhoodId,
                                                     Neighborhood.[Name] AS NeighborhoodName,
                                                     Phone
                                                   FROM [Owner]
                                                   INNER JOIN Neighborhood ON Neighborhood.Id = NeighborhoodId";
        // The constructor accepts an IConfiguration object as a parameter. This class comes from the ASP.NET framework and is useful for retrieving things out of the appsettings.json file like connection strings.
        public OwnerRepository(IConfiguration config) : base(config) { }

        public List<Owner> GetAllOwners()
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = _baseSqlSelect;

                    using (var reader = cmd.ExecuteReader())
                    {
                        var results = new List<Owner>();
                        while (reader.Read())
                        {
                            var owner = LoadFromData(reader);

                            results.Add(owner);
                        }

                        return results;
                    }
                }
            }
        }

        public Owner? GetOwnerById(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = $"{_baseSqlSelect} WHERE [Owner].Id = @Id";

                    cmd.Parameters.AddWithValue("@id", id);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        Owner? result = null;

                        if (reader.Read())
                        {
                            result = LoadFromData(reader);
                        };

                            return result;
                       
                    }
                }
            }
        }
        public void AddOwner(Owner owner)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                    INSERT INTO Owner ([Name], Email, Phone, Address, NeighborhoodId)
                    OUTPUT INSERTED.ID
                    VALUES (@name, @email, @phoneNumber, @address, @neighborhoodId);
                ";

                    cmd.Parameters.AddWithValue("@name", owner.Name);
                    cmd.Parameters.AddWithValue("@email", owner.Email);
                    cmd.Parameters.AddWithValue("@phoneNumber", owner.Phone);
                    cmd.Parameters.AddWithValue("@address", owner.Address);
                    cmd.Parameters.AddWithValue("@neighborhoodId", owner.NeighborhoodId);

                    int id = (int)cmd.ExecuteScalar();

                    owner.Id = id;
                }
            }
        }

        public void UpdateOwner(Owner owner)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                            UPDATE Owner
                            SET 
                                [Name] = @name, 
                                Email = @email, 
                                Address = @address, 
                                Phone = @phone, 
                                NeighborhoodId = @neighborhoodId
                            WHERE Id = @id";

                    cmd.Parameters.AddWithValue("@name", owner.Name);
                    cmd.Parameters.AddWithValue("@email", owner.Email);
                    cmd.Parameters.AddWithValue("@address", owner.Address);
                    cmd.Parameters.AddWithValue("@phone", owner.Phone);
                    cmd.Parameters.AddWithValue("@neighborhoodId", owner.NeighborhoodId);
                    cmd.Parameters.AddWithValue("@id", owner.Id);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void DeleteOwner(int ownerId)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                            DELETE FROM Owner
                            WHERE Id = @id
                        ";

                    cmd.Parameters.AddWithValue("@id", ownerId);

                    cmd.ExecuteNonQuery();
                }
            }
        }
private Owner LoadFromData(SqlDataReader reader)
        {
            return new Owner
            {
                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                Name = reader.GetString(reader.GetOrdinal("Name")),
                Email = reader.GetString(reader.GetOrdinal("Email")),
                Address = reader.GetString(reader.GetOrdinal("Address")),
                NeighborhoodId = reader.GetInt32(reader.GetOrdinal("NeighborhoodId")),
                Neighborhood = new Neighborhood { Name = reader.GetString(reader.GetOrdinal("NeighborhoodName")) },
                Phone = reader.GetString(reader.GetOrdinal("Phone"))
            };

        }
    }
}

