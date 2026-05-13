using RestaurantComenzi.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace RestaurantComenzi.DataAccess.Repositories
{
    public class ProdusRepository : BaseRepository
    {
        public List<Preparat> GetPreparate()
        {
            var preparate = new List<Preparat>();

            using (var connection = GetConnection())
            using (var command = new SqlCommand("sp_GetPreparate", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                connection.Open();

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        preparate.Add(new Preparat
                        {
                            Id = (int)reader["Id"],
                            Denumire = reader["Denumire"].ToString(),
                            Pret = (decimal)reader["Pret"],
                            CantitatePortie = reader["CantitatePortie"].ToString(),
                            CantitateTotala = (decimal)reader["CantitateTotala"],
                            CategorieId = (int)reader["CategorieId"],
                            Disponibil = (bool)reader["Disponibil"]
                        });
                    }
                }
            }
            return preparate;
        }

        public void UpdateStoc(int preparatId, decimal cantitateScazuta)
        {
            using (var connection = GetConnection())
            using (var command = new SqlCommand("sp_UpdateStocPreparat", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@PreparatId", preparatId);
                command.Parameters.AddWithValue("@CantitateScazuta", cantitateScazuta);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public void SetIndisponibil(int preparatId)
        {
            using (var connection = GetConnection())
            using (var command = new SqlCommand("sp_SetPreparatIndisponibil", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@PreparatId", preparatId);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }
    }
}