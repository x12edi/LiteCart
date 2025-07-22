using ECommerce.Domain.Entities;
using ECommerce.Domain.Repositories;
using Microsoft.Data.SqlClient;
using System.Data;

namespace ECommerce.Infrastructure.Repositories
{
    public class WarehouseRepository : IRepository<Warehouse>
    {
        private readonly string _connectionString;

        public WarehouseRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<Warehouse> GetByIdAsync(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            var command = new SqlCommand("SELECT * FROM Warehouses WHERE Id = @Id", connection);
            command.Parameters.AddWithValue("@Id", id);

            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new Warehouse
                {
                    Id = reader.GetInt32("Id"),
                    Location = reader.GetString("Location"),
                    Manager = reader["Manager"] != DBNull.Value ? reader.GetString("Manager") : null,
                    Capacity = reader.GetInt32("Capacity")
                };
            }
            return null;
        }

        public async Task<IEnumerable<Warehouse>> GetAllAsync()
        {
            var warehouses = new List<Warehouse>();
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            var command = new SqlCommand("SELECT * FROM Warehouses", connection);

            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                warehouses.Add(new Warehouse
                {
                    Id = reader.GetInt32("Id"),
                    Location = reader.GetString("Location"),
                    Manager = reader["Manager"] != DBNull.Value ? reader.GetString("Manager") : null,
                    Capacity = reader.GetInt32("Capacity")
                });
            }
            return warehouses;
        }

        public async Task AddAsync(Warehouse entity)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            var command = new SqlCommand(
                "INSERT INTO Warehouses (Location, Manager, Capacity) " +
                "VALUES (@Location, @Manager, @Capacity)", connection);
            command.Parameters.AddWithValue("@Location", entity.Location);
            command.Parameters.AddWithValue("@Manager", (object)entity.Manager ?? DBNull.Value);
            command.Parameters.AddWithValue("@Capacity", entity.Capacity);

            await command.ExecuteNonQueryAsync();
        }

        public async Task UpdateAsync(Warehouse entity)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            var command = new SqlCommand(
                "UPDATE Warehouses SET Location = @Location, Manager = @Manager, Capacity = @Capacity WHERE Id = @Id", connection);
            command.Parameters.AddWithValue("@Id", entity.Id);
            command.Parameters.AddWithValue("@Location", entity.Location);
            command.Parameters.AddWithValue("@Manager", (object)entity.Manager ?? DBNull.Value);
            command.Parameters.AddWithValue("@Capacity", entity.Capacity);

            await command.ExecuteNonQueryAsync();
        }

        public async Task DeleteAsync(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            var command = new SqlCommand("DELETE FROM Warehouses WHERE Id = @Id", connection);
            command.Parameters.AddWithValue("@Id", id);

            await command.ExecuteNonQueryAsync();
        }
    }
}