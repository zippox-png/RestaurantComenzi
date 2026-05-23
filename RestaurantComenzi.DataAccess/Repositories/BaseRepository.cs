using System.Configuration;
using System.Data.SqlClient;

namespace RestaurantComenzi.DataAccess.Repositories
{
    public abstract class BaseRepository
    {
        private readonly string _connectionString;

        protected BaseRepository()
        {
            var connectionSetting = ConfigurationManager.ConnectionStrings["RestaurantData"];
            _connectionString = connectionSetting != null ? connectionSetting.ConnectionString : "Server=(localdb)\\MSSQLLocalDB;Database=RestaurantData;Trusted_Connection=True;";
        }

        protected SqlConnection GetConnection()
        {
            return new SqlConnection(_connectionString);
        }
    }
}