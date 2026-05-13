using System.Data;
using System.Data.SqlClient;
using RestaurantComenzi.DataAccess.Entities;

namespace RestaurantComenzi.DataAccess.Repositories
{
    public class UtilizatorRepository : BaseRepository
    {
        public Utilizator Login(string email, string parola)
        {
            using (var connection = GetConnection())
            using (var command = new SqlCommand("sp_LoginUtilizator", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Email", email);
                command.Parameters.AddWithValue("@Parola", parola);

                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new Utilizator
                        {
                            Id = (int)reader["Id"],
                            Nume = reader["Nume"].ToString(),
                            Prenume = reader["Prenume"].ToString(),
                            Email = reader["Email"].ToString(),
                            Rol = reader["Rol"].ToString()
                        };
                    }
                }
            }
            return null;
        }

        public void Register(Utilizator u)
        {
            using (var connection = GetConnection())
            using (var command = new SqlCommand("sp_InsertUtilizator", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Nume", u.Nume);
                command.Parameters.AddWithValue("@Prenume", u.Prenume);
                command.Parameters.AddWithValue("@Email", u.Email);
                command.Parameters.AddWithValue("@Parola", u.Parola);
                command.Parameters.AddWithValue("@Telefon", u.Telefon);
                command.Parameters.AddWithValue("@Adresa", u.AdresaLivrare);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }
    }
}