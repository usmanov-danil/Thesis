using System.Data.SQLite;
using System.IO;

namespace Aggregator.repositories
{
    class Sqlite : DbRepository
    {
        private string DbPath = "MyDatabase.sqlite";
        private SQLiteConnection conn;
        public Sqlite()
        {
            if (!File.Exists(this.DbPath))
                SQLiteConnection.CreateFile(this.DbPath);

            SQLiteConnection conn = new SQLiteConnection(string.Format("Data Source={1};Version=3;", this.DbPath));
            conn.Open();

            string query = "create table if not exists highscores (name varchar(20), score int)";  // FIXME
            SQLiteCommand command = new SQLiteCommand(query, conn);
            command.ExecuteNonQuery();
            conn.Close();

        }

        public void AddData()
        {
            conn.Open();
            string sql = "insert into highscores (name, score) values ('Me', 3000)";
            SQLiteCommand command = new SQLiteCommand(sql, conn);
            command.ExecuteNonQuery();
            conn.Close();
        }

        public void GetData()
        {
            conn.Open();
            string sql = "select * from highscores order by score desc";
            SQLiteCommand command = new SQLiteCommand(sql, conn);
            SQLiteDataReader reader = command.ExecuteReader();
            //while (reader.Read())
              //Console.WriteLine("Name: " + reader["name"] + "\tScore: " + reader["score"]);
            conn.Close();
        }

    }
}
