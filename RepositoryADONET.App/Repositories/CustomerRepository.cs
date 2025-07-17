using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using RepositoryADONET.App.Models;

namespace RepositoryADONET.App.Repositories;

public class CustomerRepository : RepositoryBase<Customer>
{
    public CustomerRepository(IConfiguration configuration) : base(configuration) { }

    protected override Customer MapDataBaseToEntity(SqlDataReader reader)
    {
        var customer = new Customer
        {
            Id = reader.GetInt32(reader.GetOrdinal("Id")),
            Name = reader.GetString(reader.GetOrdinal("Name")),
            Email = reader.GetString(reader.GetOrdinal("Email"))
        };

        return customer;
    }

    public override int Insert(Customer customer, string tableName)
    {
        using var connection = CreateConnection();
        connection.Open();

        var command = new SqlCommand($"INSERT INTO {tableName} (Name, Email) VALUES (@Name, @Email)", connection);
        command.Parameters.AddWithValue("@Name", customer.Name);
        command.Parameters.AddWithValue("@Email", customer.Email);

        return command.ExecuteNonQuery();
    }

    public override int Update(Customer customer, string tableName)
    {
        using var connection = CreateConnection();
        connection.Open();

        var command = new SqlCommand($"UPDATE {tableName} SET Name = @Name, Email = @Email WHERE Id = @Id", connection);
        command.Parameters.AddWithValue("@Name", customer.Name);
        command.Parameters.AddWithValue("@Email", customer.Email);
        command.Parameters.AddWithValue("@Id", customer.Id);

        return command.ExecuteNonQuery();
    }
}
