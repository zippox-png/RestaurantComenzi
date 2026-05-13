using RestaurantComenzi.DataAccess.Entities;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace RestaurantComenzi.DataAccess.Repositories
{
    public class AlergenRepository : BaseRepository
    {
        // SELECT - Pentru afișarea listei de alergeni în interfață
        public List<Alergen> GetAllAlergeni()
        {
            var alergeni = new List<Alergen>();

            using (var connection = GetConnection())
            using (var command = new SqlCommand("sp_GetAllAlergeni", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                connection.Open();

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        alergeni.Add(new Alergen
                        {
                            Id = (int)reader["Id"],
                            Denumire = reader["Denumire"].ToString()
                        });
                    }
                }
            }
            return alergeni;
        }

        // INSERT - Pentru funcționalitatea de adăugare (Angajat)
        public void InsertAlergen(string denumire)
        {
            using (var connection = GetConnection())
            using (var command = new SqlCommand("sp_InsertAlergen", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Denumire", denumire);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public void AsociazaAlergenLaPreparat(int preparatId, int alergenId)
        {
            using (var connection = GetConnection())
            using (var command = new SqlCommand("sp_AsociazaAlergen", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@PreparatId", preparatId);
                command.Parameters.AddWithValue("@AlergenId", alergenId);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }
    }
}