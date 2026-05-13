using System.Configuration;
using System.Data.SqlClient;

namespace RestaurantComenzi.DataAccess.Repositories
{
    public abstract class BaseRepository
    {
        private readonly string _connectionString;

        protected BaseRepository()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["RestaurantDB"].ConnectionString;
        }

        protected SqlConnection GetConnection()
        {
            return new SqlConnection(_connectionString);
        }
    }
}