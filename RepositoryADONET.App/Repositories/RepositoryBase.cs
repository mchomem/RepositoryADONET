namespace RepositoryADONET.App.Repositories;

public class RepositoryBase<T>: IRepositoryBase<T> where T : class, new()
{
    private readonly string _connectionString;

    public RepositoryBase(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection")!;
    }

    protected SqlConnection CreateConnection()
    {
        return new SqlConnection(_connectionString);
    }

    public T MapDataBaseToEntity(SqlDataReader reader)
    {
        var entity = new T();
        var entityType = typeof(T);

        foreach (var property in entityType.GetProperties())
        {
            if (!reader.HasColumn(property.Name) || !property.CanWrite)
                continue;

            var value = reader[property.Name];

            if (value == DBNull.Value)
                property.SetValue(entity, null);
            else
            {
                var targetType = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;
                var safeValue = Convert.ChangeType(value, targetType);
                property.SetValue(entity, safeValue);
            }
        }

        return entity;
    }

    public async Task<T> GetrByIdAsync(int id)
    {
        using var connection = CreateConnection();
        await connection.OpenAsync();

        var command = new SqlCommand($"select * from {typeof(T).Name} where Id = @Id", connection);
        command.Parameters.AddWithValue("@Id", id);

        using var reader = await command.ExecuteReaderAsync();

        if (await reader.ReadAsync())
        {
            return MapDataBaseToEntity(reader);
        }

        return null!;
    }

    public async Task<List<T>> GetAllAsync()
    {
        var list = new List<T>();

        using var connection = CreateConnection();
        await connection.OpenAsync();

        var command = new SqlCommand($"select * from {typeof(T).Name}", connection);
        using var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            list.Add(MapDataBaseToEntity(reader));
        }

        return list;
    }

    public async Task<int> InserAsync(T entity)
    {
        using var connection = CreateConnection();
        await connection.OpenAsync();

        var properties = typeof(T).GetProperties()
            .Where(p => p.CanRead && p.Name.ToLower() != "id")
            .ToList();

        var columnsNames = string.Join(", ", properties.Select(p => p.Name));
        var parametersNames = string.Join(", ", properties.Select(p => $"@{p.Name}"));
        var commandTextPlain = $"insert into {typeof(T).Name} ({columnsNames}) values ({parametersNames})";
        using var command = new SqlCommand(commandTextPlain, connection);

        foreach (var prop in properties)
        {
            var value = prop.GetValue(entity) ?? DBNull.Value;
            command.Parameters.AddWithValue($"@{prop.Name}", value);
        }

        return command.ExecuteNonQuery();
    }

    public async Task<int> UpdateAsync(T entity)
    {
        using var connection = CreateConnection();
        await connection.OpenAsync();

        var properties = typeof(T).GetProperties()
            .Where(p => p.CanRead && p.Name.ToLower() != "id")
            .ToList();

        var idProperty = typeof(T).GetProperties().Where(p => p.CanRead).FirstOrDefault()?.Name ?? "Id";

        var columnsNames = properties.Select(p => p.Name);

        var commandTextPlain = $"update {typeof(T).Name} set ";
        var filedsToUpdated = new List<string>();

        foreach (var columnName in columnsNames)
        {
            filedsToUpdated.Add($"{columnName} = @{columnName}");
        }

        commandTextPlain += string.Join(", ", filedsToUpdated);

        commandTextPlain += $" where {idProperty} = @{idProperty}";

        using var command = new SqlCommand(commandTextPlain, connection);

        foreach (var prop in properties)
        {
            var value = prop.GetValue(entity) ?? DBNull.Value;
            command.Parameters.AddWithValue($"@{prop.Name}", value);
        }

        var valueId = typeof(T).GetProperty(idProperty)?.GetValue(entity) ?? DBNull.Value;
        command.Parameters.AddWithValue($"@{idProperty}", valueId);

        return command.ExecuteNonQuery();
    }

    public async Task<int> DeleteAsync(T entity)
    {
        using var connection = CreateConnection();
        await connection.OpenAsync();

        var idProperty = typeof(T).GetProperties().Where(p => p.CanRead).FirstOrDefault()?.Name ?? "Id";

        var command = new SqlCommand($"delete from {typeof(T).Name} where Id = @Id", connection);

        var valueId = typeof(T).GetProperty(idProperty)?.GetValue(entity) ?? DBNull.Value;
        command.Parameters.AddWithValue($"@{idProperty}", valueId);

        return await command.ExecuteNonQueryAsync();
    }
}
