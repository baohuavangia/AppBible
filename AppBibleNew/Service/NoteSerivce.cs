using AppBibleNew.Model;

using Microsoft.Data.Sqlite;

namespace AppBibleNew.Service
{
    public class NoteService
    {
        private readonly string _dbPath;

        public NoteService(string dbPath)
        {
            _dbPath = dbPath;
            using var con = new SqliteConnection($"Data Source={_dbPath}");
            con.Open();
            var cmd = con.CreateCommand();
            cmd.CommandText = @"CREATE TABLE IF NOT EXISTS Notes(
                                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                                Title TEXT,
                                Content TEXT,
                                CreatedAt TEXT)";
            cmd.ExecuteNonQuery();
        }

        public List<Note> GetAll()
        {
            var notes = new List<Note>();
            using var con = new SqliteConnection($"Data Source={_dbPath}");
            con.Open();
            var cmd = con.CreateCommand();
            cmd.CommandText = "SELECT Id, Title, Content, CreatedAt FROM Notes ORDER BY CreatedAt DESC";
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                notes.Add(new Note
                {
                    Id = reader.GetInt32(0),
                    Title = reader.GetString(1),
                    Content = reader.GetString(2),
                    CreatedAt = DateTime.Parse(reader.GetString(3))
                });
            }
            return notes;
        }

        public void Save(Note note)
        {
            using var con = new SqliteConnection($"Data Source={_dbPath}");
            con.Open();
            var cmd = con.CreateCommand();

            if (note.Id == 0)
            {
                cmd.CommandText = "INSERT INTO Notes (Title,Content,CreatedAt) VALUES (@t,@c,@d)";
                cmd.Parameters.AddWithValue("@d", note.CreatedAt.ToString("s"));
            }
            else
            {
                cmd.CommandText = "UPDATE Notes SET Title=@t, Content=@c WHERE Id=@id";
                cmd.Parameters.AddWithValue("@id", note.Id);
            }

            cmd.Parameters.AddWithValue("@t", note.Title);
            cmd.Parameters.AddWithValue("@c", note.Content);
            cmd.ExecuteNonQuery();
        }

        public void Delete(int id)
        {
            using var con = new SqliteConnection($"Data Source={_dbPath}");
            con.Open();
            var cmd = con.CreateCommand();
            cmd.CommandText = "DELETE FROM Notes WHERE Id=@id";
            cmd.Parameters.AddWithValue("@id", id);
            cmd.ExecuteNonQuery();
        }
    }
}
