using RestaurantComenzi.DataAccess.Entities;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace RestaurantComenzi.DataAccess.Repositories
{
    public class CategorieRepository : BaseRepository
    {
        public List<Categorie> GetCategorii()
        {
            var categorii = new List<Categorie>();

            using (var connection = GetConnection())
            using (var command = new SqlCommand("sp_GetCategorii", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                connection.Open();

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        categorii.Add(new Categorie
                        {
                            Id = (int)reader["Id"],
                            Denumire = reader["Denumire"].ToString()
                        });
                    }
                }
            }
            return categorii;
        }

        public void InsertCategorie(string denumire)
        {
            using (var connection = GetConnection())
            using (var command = new SqlCommand("sp_InsertCategorie", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Denumire", denumire);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }
    }
}