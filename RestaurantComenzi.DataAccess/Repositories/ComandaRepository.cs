using RestaurantComenzi.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace RestaurantComenzi.DataAccess.Repositories
{
    public class ComandaRepository : BaseRepository
    {
        public int InsertComanda(Comanda comanda)
        {
            using (var connection = GetConnection())
            using (var command = new SqlCommand("sp_InsertComanda", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@UtilizatorId", comanda.UtilizatorId);
                command.Parameters.AddWithValue("@PretTotal", comanda.PretTotal);
                command.Parameters.AddWithValue("@CostTransport", comanda.CostTransport);
                command.Parameters.AddWithValue("@OraEstimativa", (object)comanda.OraEstimativaLivrare ?? DBNull.Value);

                connection.Open();
                // Returnează ID-ul noii comenzi
                return Convert.ToInt32(command.ExecuteScalar());
            }
        }

        public List<Comanda> GetComenziClient(int utilizatorId)
        {
            var comenzi = new List<Comanda>();

            using (var connection = GetConnection())
            using (var command = new SqlCommand("sp_GetComenziClient", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@UtilizatorId", utilizatorId);

                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        comenzi.Add(new Comanda
                        {
                            Id = (int)reader["Id"],
                            CodComanda = (Guid)reader["CodComanda"],
                            DataComanda = (DateTime)reader["DataComanda"],
                            Stare = reader["Stare"].ToString(),
                            PretTotal = (decimal)reader["PretTotal"],
                            CostTransport = (decimal)reader["CostTransport"],
                            OraEstimativaLivrare = reader["OraEstimativaLivrare"] != DBNull.Value ? (DateTime?)reader["OraEstimativaLivrare"] : null
                        });
                    }
                }
            }
            return comenzi;
        }

        // METODA NOUĂ PENTRU ANGAJAȚI
        public List<Comanda> GetAllComenzi()
        {
            var comenzi = new List<Comanda>();

            using (var connection = GetConnection())
            using (var command = new SqlCommand("sp_GetAllComenzi", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        comenzi.Add(new Comanda
                        {
                            Id = (int)reader["Id"],
                            CodComanda = (Guid)reader["CodComanda"],
                            DataComanda = (DateTime)reader["DataComanda"],
                            Stare = reader["Stare"].ToString(),
                            PretTotal = (decimal)reader["PretTotal"],
                            CostTransport = (decimal)reader["CostTransport"],
                            OraEstimativaLivrare = reader["OraEstimativaLivrare"] != DBNull.Value ? (DateTime?)reader["OraEstimativaLivrare"] : null
                        });
                    }
                }
            }
            return comenzi;
        }

        public void UpdateStare(int comandaId, string stareNoua)
        {
            using (var connection = GetConnection())
            using (var command = new SqlCommand("sp_UpdateStareComanda", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@ComandaId", comandaId);
                command.Parameters.AddWithValue("@StareNoua", stareNoua);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }
        public void InsertDetaliuComanda(ComandaItem item)
        {
            using (var connection = GetConnection())
            using (var command = new System.Data.SqlClient.SqlCommand("INSERT INTO DetaliiComanda (ComandaId, PreparatId, MeniuId, Cantitate) VALUES (@CId, @PId, @MId, @Cant)", connection))
            {
                command.Parameters.AddWithValue("@CId", item.ComandaId);
                command.Parameters.AddWithValue("@PId", (object)item.PreparatId ?? DBNull.Value);
                command.Parameters.AddWithValue("@MId", (object)item.MeniuId ?? DBNull.Value);
                command.Parameters.AddWithValue("@Cant", item.Cantitate);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }
    }
}