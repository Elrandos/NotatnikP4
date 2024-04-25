using System.Data.SqlClient;
using Dapper;
using WPF_lab3.Dto;
using WPF_lab3.Model;

namespace WPF_lab3.Persistence
{
    public class AppDbContext
    {
        private static readonly string DB_NAME = "RB_Gr1";

        private readonly SqlConnection _sqlConnection;

        public AppDbContext(string connectionString)
        {
            _sqlConnection = new SqlConnection(connectionString);
            _sqlConnection.Open();
            CreateAppDbContext();
        }
        public void CreateAppDbContext()
        {
            CreateDatabaseIfNotExist();
            CreateTableIfNotExist();
        }

        public IEnumerable<T> GetFromDatabase<T>(string query)
        {
            return _sqlConnection.Query<T>(query);
        }
        public void CreateDatabaseIfNotExist()
        {
            var command = "IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = '" + DB_NAME + "') BEGIN CREATE DATABASE " + DB_NAME + "; END;";
            SqlCommand sqlCommand = new SqlCommand(command);
            sqlCommand.Connection = _sqlConnection;
            sqlCommand.ExecuteNonQuery();
            ChangeDatabase();
        }

        public void ChangeDatabase()
        {
            _sqlConnection.ChangeDatabase(DB_NAME);
        }

        public void CreateTableIfNotExist()
        {
            var createNotesTableCommand = """
                                          IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Notes' AND type = 'U')
                                          BEGIN
                                              CREATE TABLE Notes(
                                                  Id INT IDENTITY(1,1) PRIMARY KEY,
                                                  Name NVARCHAR(255),
                                                  Description NVARCHAR(MAX),
                                                  UserId INT NOT NULL
                                              );
                                          END;
                                          """;
            var createUsersTableCommand = """
                                          IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Users' AND type = 'U')
                                          BEGIN
                                              CREATE TABLE Users(
                                                  Id INT IDENTITY(1,1) PRIMARY KEY,
                                                  Username NVARCHAR(255),
                                                  Password NVARCHAR(255)
                                              );
                                          END;
                                          """;
            ExecuteSqlCommand(createNotesTableCommand);
            ExecuteSqlCommand(createUsersTableCommand);
        }
        private void ExecuteSqlCommand(string commandText)
        {
            SqlCommand sqlCommand = new SqlCommand(commandText);
            sqlCommand.Connection = _sqlConnection;
            sqlCommand.ExecuteNonQuery();
        }

        //------------------------------------------------
        //TASK
        public void CreateNote(NoteDto noteDto)
        {
            var insertCommand = "INSERT INTO Notes (Name, Description, UserId) VALUES (@Name, @Description, @UserId);";
            _sqlConnection.Execute(insertCommand, noteDto);
        }

        public void EditNote(Note note)
        {
            var updateCommand = "UPDATE Notes SET Name = @Name, Description = @Description WHERE Id = @Id;";
            _sqlConnection.Execute(updateCommand, note);
        }

        public void DeleteNote(int noteId)
        {
            var deleteCommand = "DELETE FROM Notes WHERE Id = @Id;";
            _sqlConnection.Execute(deleteCommand, new { Id = noteId });
        }

        //------------------------------------------------
        //USER

        public void CreateUser(UserDto user)
        {
            var insertCommand = "INSERT INTO Users VALUES (@Login, @Password);";
            _sqlConnection.Execute(insertCommand, user);
        }

        public User? GetUserByUsername(string username)
        {
            var query = "SELECT TOP 1 * FROM Users WHERE Username = @Username;";
            return _sqlConnection.QueryFirstOrDefault<User>(query, new { Username = username });
        }
    }
}
