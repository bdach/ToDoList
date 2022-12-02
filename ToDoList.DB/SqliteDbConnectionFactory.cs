using System.Data;
using Microsoft.Data.Sqlite;

namespace ToDoList.DB;

public class SqliteDbConnectionFactory : IDbConnectionFactory
{
    private readonly string _connectionString;

    public SqliteDbConnectionFactory(string connectionString)
    {
        _connectionString = connectionString;
    }

    public IDbConnection Create()
    {
        return new SqliteConnection(_connectionString);
    }
}