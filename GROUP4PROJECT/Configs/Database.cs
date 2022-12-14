using Npgsql;

namespace GROUP4PROJECT.Configs
{
    public class Database
    {
        public static string GetConnectionString()
        {
            string host = SupabaseConfig.db_host;
            string username = "postgres";
            string password = SupabaseConfig.password;
            string database = "postgres";
            return $"Host=db.{host};Username={username};password={password};Database={database}";
        }

        public static NpgsqlConnection GetConnection()
        {
            NpgsqlConnection connection = new NpgsqlConnection(GetConnectionString());

            if (connection.State == System.Data.ConnectionState.Open)
            {
                connection.Close();
            }

            connection.Open();

            return connection;
        }
    }
}