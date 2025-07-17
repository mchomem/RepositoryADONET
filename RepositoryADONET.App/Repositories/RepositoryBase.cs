using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace RepositoryADONET.App.Repositories;

public abstract class RepositoryBase<T> where T : class, new()
{
    private readonly string _connectionString;

    protected RepositoryBase(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection")!;
    }

    protected SqlConnection CreateConnection()
    {
        return new SqlConnection(_connectionString);
    }

    protected abstract T MapDataBaseToEntity(SqlDataReader reader);

    public virtual async Task<T> GetrByIdAsync(int id, string tableName)
    {
        using var connection = CreateConnection();
        await connection.OpenAsync();

        var command = new SqlCommand($"SELECT * FROM {tableName} WHERE Id = @Id", connection);
        command.Parameters.AddWithValue("@Id", id);

        using var reader = await command.ExecuteReaderAsync();

        if (await reader.ReadAsync())
        {
            return MapDataBaseToEntity(reader);
        }

        return null!;
    }

    public virtual async Task<List<T>> GetAllAsync(string tableName)
    {
        var list = new List<T>();

        using var connection = CreateConnection();
        await connection.OpenAsync();

        var command = new SqlCommand($"SELECT * FROM {tableName}", connection);
        using var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            list.Add(MapDataBaseToEntity(reader));
        }

        return list;
    }

    public abstract int Insert(T entity, string tableName);

    public abstract int Update(T entity, string tableName);

    public virtual int Delete(int id, string tableName)
    {
        using var connection = CreateConnection();
        connection.Open();

        var command = new SqlCommand($"DELETE FROM {tableName} WHERE Id = @Id", connection);
        command.Parameters.AddWithValue("@Id", id);

        return command.ExecuteNonQuery();
    }
}
