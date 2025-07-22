using ECommerce.Domain.Entities;
using ECommerce.Domain.Repositories;
using Microsoft.Data.SqlClient;
using System.Data;

namespace ECommerce.Infrastructure.Repositories
{
    public class OrderRepository : IRepository<Order>
    {
        private readonly string _connectionString;

        public OrderRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<Order> GetByIdAsync(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            var command = new SqlCommand("SELECT * FROM Orders WHERE Id = @Id", connection);
            command.Parameters.AddWithValue("@Id", id);

            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new Order
                {
                    Id = reader.GetInt32("Id"),
                    UserId = reader.GetInt32("UserId"),
                    TotalAmount = reader.GetDecimal("TotalAmount"),
                    Status = reader.GetString("Status"),
                    CreatedAt = reader.GetDateTime("CreatedAt")
                };
            }
            return null;
        }

        public async Task<IEnumerable<Order>> GetAllAsync()
        {
            var orders = new List<Order>();
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            var command = new SqlCommand("SELECT * FROM Orders", connection);

            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                orders.Add(new Order
                {
                    Id = reader.GetInt32("Id"),
                    UserId = reader.GetInt32("UserId"),
                    TotalAmount = reader.GetDecimal("TotalAmount"),
                    Status = reader.GetString("Status"),
                    CreatedAt = reader.GetDateTime("CreatedAt")
                });
            }
            return orders;
        }

        public async Task AddAsync(Order entity)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            var command = new SqlCommand(
                "INSERT INTO Orders (UserId, TotalAmount, Status, CreatedAt) " +
                "VALUES (@UserId, @TotalAmount, @Status, @CreatedAt)", connection);
            command.Parameters.AddWithValue("@UserId", entity.UserId);
            command.Parameters.AddWithValue("@TotalAmount", entity.TotalAmount);
            command.Parameters.AddWithValue("@Status", entity.Status);
            command.Parameters.AddWithValue("@CreatedAt", entity.CreatedAt);

            await command.ExecuteNonQueryAsync();
        }

        public async Task UpdateAsync(Order entity)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            var command = new SqlCommand(
                "UPDATE Orders SET UserId = @UserId, TotalAmount = @TotalAmount, Status = @Status WHERE Id = @Id", connection);
            command.Parameters.AddWithValue("@Id", entity.Id);
            command.Parameters.AddWithValue("@UserId", entity.UserId);
            command.Parameters.AddWithValue("@TotalAmount", entity.TotalAmount);
            command.Parameters.AddWithValue("@Status", entity.Status);

            await command.ExecuteNonQueryAsync();
        }

        public async Task DeleteAsync(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            var command = new SqlCommand("DELETE FROM Orders WHERE Id = @Id", connection);
            command.Parameters.AddWithValue("@Id", id);

            await command.ExecuteNonQueryAsync();
        }
    }
}