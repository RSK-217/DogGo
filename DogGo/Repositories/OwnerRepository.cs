using DogGo.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Drawing.Text;

namespace DogGo.Repositories
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
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = _baseSqlSelect;

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        List<Owner> owners = new List<Owner>();
                        while (reader.Read())
                        {
                            Owner owner = LoadFromData(reader);

                            owners.Add(owner);
                        }

                        return owners;
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

