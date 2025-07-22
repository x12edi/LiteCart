using ECommerce.Domain.Entities;
using ECommerce.Domain.Repositories;
using Microsoft.Data.SqlClient;
using System.Data;

namespace ECommerce.Infrastructure.Repositories
{
    public class AddressRepository : IRepository<Address>
    {
        private readonly string _connectionString;

        public AddressRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<Address> GetByIdAsync(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            var command = new SqlCommand("SELECT * FROM Addresses WHERE Id = @Id", connection);
            command.Parameters.AddWithValue("@Id", id);

            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new Address
                {
                    Id = reader.GetInt32("Id"),
                    UserId = reader.GetInt32("UserId"),
                    Street = reader.GetString("Street"),
                    City = reader.GetString("City"),
                    State = reader["State"] != DBNull.Value ? reader.GetString("State") : null,
                    Zip = reader.GetString("Zip"),
                    Country = reader.GetString("Country"),
                    IsDefault = reader.GetBoolean("IsDefault")
                };
            }
            return null;
        }

        public async Task<IEnumerable<Address>> GetAllAsync()
        {
            var addresses = new List<Address>();
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            var command = new SqlCommand("SELECT * FROM Addresses", connection);

            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                addresses.Add(new Address
                {
                    Id = reader.GetInt32("Id"),
                    UserId = reader.GetInt32("UserId"),
                    Street = reader.GetString("Street"),
                    City = reader.GetString("City"),
                    State = reader["State"] != DBNull.Value ? reader.GetString("State") : null,
                    Zip = reader.GetString("Zip"),
                    Country = reader.GetString("Country"),
                    IsDefault = reader.GetBoolean("IsDefault")
                });
            }
            return addresses;
        }

        public async Task AddAsync(Address entity)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            var command = new SqlCommand(
                "INSERT INTO Addresses (UserId, Street, City, State, Zip, Country, IsDefault) " +
                "VALUES (@UserId, @Street, @City, @State, @Zip, @Country, @IsDefault)", connection);
            command.Parameters.AddWithValue("@UserId", entity.UserId);
            command.Parameters.AddWithValue("@Street", entity.Street);
            command.Parameters.AddWithValue("@City", entity.City);
            command.Parameters.AddWithValue("@State", (object)entity.State ?? DBNull.Value);
            command.Parameters.AddWithValue("@Zip", entity.Zip);
            command.Parameters.AddWithValue("@Country", entity.Country);
            command.Parameters.AddWithValue("@IsDefault", entity.IsDefault);

            await command.ExecuteNonQueryAsync();
        }

        public async Task UpdateAsync(Address entity)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            var command = new SqlCommand(
                "UPDATE Addresses SET UserId = @UserId, Street = @Street, City = @City, State = @State, " +
                "Zip = @Zip, Country = @Country, IsDefault = @IsDefault WHERE Id = @Id", connection);
            command.Parameters.AddWithValue("@Id", entity.Id);
            command.Parameters.AddWithValue("@UserId", entity.UserId);
            command.Parameters.AddWithValue("@Street", entity.Street);
            command.Parameters.AddWithValue("@City", entity.City);
            command.Parameters.AddWithValue("@State", (object)entity.State ?? DBNull.Value);
            command.Parameters.AddWithValue("@Zip", entity.Zip);
            command.Parameters.AddWithValue("@Country", entity.Country);
            command.Parameters.AddWithValue("@IsDefault", entity.IsDefault);

            await command.ExecuteNonQueryAsync();
        }

        public async Task DeleteAsync(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            var command = new SqlCommand("DELETE FROM Addresses WHERE Id = @Id", connection);
            command.Parameters.AddWithValue("@Id", id);

            await command.ExecuteNonQueryAsync();
        }
    }
}