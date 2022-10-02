using Npgsql;

namespace GROUP4PROJECT.Configs
{
    public class Database
    {
        public static string GetConnectionString()
        {
            string host = "db.lmchozvtyhzraokprqdq.supabase.co";
            string username = "postgres";
            string password = "xtTNvEdWTMB7QFb4";
            string database = "postgres";
            return $"Host={host};Username={username};password={password};Database={database}";
        }

        public static NpgsqlConnection GetConnection()
        {
            string connectionString = GetConnectionString();
            NpgsqlConnection connection = new NpgsqlConnection(connectionString);
            connection.Open();

            return connection;
        }
    }
}