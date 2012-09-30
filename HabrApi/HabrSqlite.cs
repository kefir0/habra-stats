using System.Data.SQLite;

namespace HabrApi
{
    public class HabrSqlite
    {
        private readonly Habr _habr;
        const string SqliteDbFile = @"e:\HabraStats.sqlite";
        private const string DbScript = @"create table if not exists Posts (id integer not null primary key, html nvarchar not null);"+
            "create table if not exists Comments (id integer not null primary key, text nvarchar not null);";

        public HabrSqlite()
        {
            _habr = new Habr();
        }

        public void LoadIntoSqlite(int postId)
        {
            EnsureDb();
            var post = _habr.DownloadPost(postId);
        }

        private static void EnsureDb()
        {
            using (var connection = new SQLiteConnection(ConnString))
            {
                connection.Open();
                using (var cmd = new SQLiteCommand(DbScript, connection))
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }

        private static string ConnString
        {
            get { return "Data Source=" + SqliteDbFile + ";"; }
        }
    }
}