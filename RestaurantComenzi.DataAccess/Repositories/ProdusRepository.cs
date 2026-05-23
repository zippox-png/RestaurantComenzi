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
            var dict = new Dictionary<int, Preparat>();

            using (var connection = GetConnection())
            using (var command = new SqlCommand(@"
    SELECT p.Id AS PreparatId,
           p.Denumire,
           p.Pret,
           p.CantitatePortie,
           p.CantitateTotala,
           p.Disponibil,
           pa.AlergenId,
           a.Denumire AS AlergenDenumire
    FROM Preparate p
    LEFT JOIN PreparatAlergen pa ON pa.PreparatId = p.Id
    LEFT JOIN Alergeni a ON a.Id = pa.AlergenId
", connection))
            {
                connection.Open();

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int preparatId = Convert.ToInt32(reader["PreparatId"]);

                        if (!dict.ContainsKey(preparatId))
                        {
                            dict[preparatId] = new Preparat
                            {
                                Id = preparatId,
                                Denumire = reader["Denumire"].ToString(),
                                Pret = Convert.ToDecimal(reader["Pret"]),
                                CantitatePortie = reader["CantitatePortie"].ToString(),
                                CantitateTotala = Convert.ToDecimal(reader["CantitateTotala"]),
                                Disponibil = Convert.ToBoolean(reader["Disponibil"]),
                                Alergeni = new List<Alergen>()
                            };
                        }

                        if (reader["AlergenId"] != DBNull.Value)
                        {
                            dict[preparatId].Alergeni.Add(new Alergen
                            {
                                Id = (int)reader["AlergenId"],
                                Denumire = reader["AlergenDenumire"].ToString()
                            });
                        }
                    }
                }
            }

            return dict.Values.ToList();
        }
        public List<Alergen> GetAlergeniForPreparat(int preparatId)
        {
            var list = new List<Alergen>();

            using (var connection = GetConnection())
            using (var command = new SqlCommand(@"
        SELECT a.Id, a.Denumire
        FROM Alergeni a
        INNER JOIN PreparatAlergen pa ON pa.AlergenId = a.Id
        WHERE pa.PreparatId = @id
    ", connection))
            {
                command.CommandType = System.Data.CommandType.Text;
                command.Parameters.AddWithValue("@id", preparatId);

                connection.Open();

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new Alergen
                        {
                            Id = (int)reader["Id"],
                            Denumire = reader["Denumire"].ToString()
                        });
                    }
                }
            }

            return list;
        }

        public List<Meniu> GetMeniuri()
        {
            var meniuri = new List<Meniu>();
            using (var connection = GetConnection())
            using (var command = new SqlCommand("SELECT Id, Denumire, CategorieId, Disponibil, Pret FROM Meniuri", connection))
            {
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        meniuri.Add(new Meniu
                        {
                            Id = (int)reader["Id"],
                            Denumire = reader["Denumire"].ToString(),
                            CategorieId = reader["CategorieId"] != DBNull.Value ? (int)reader["CategorieId"] : 0,
                            Disponibil = Convert.ToBoolean(reader["Disponibil"]),
                            Pret = reader["Pret"] != DBNull.Value ? (int)Convert.ToDecimal(reader["Pret"]) : 0
                        });
                    }
                }
            }
            return meniuri;
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