using ECommerce.Domain.Entities;
using ECommerce.Domain.Repositories;
using Microsoft.Data.SqlClient;
using System.Data;

namespace ECommerce.Infrastructure.Repositories
{
    public class OrderItemRepository : IRepository<OrderItem>
    {
        private readonly string _connectionString;

        public OrderItemRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<OrderItem> GetByIdAsync(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            var command = new SqlCommand("SELECT * FROM OrderItems WHERE Id = @Id", connection);
            command.Parameters.AddWithValue("@Id", id);

            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new OrderItem
                {
                    Id = reader.GetInt32("Id"),
                    OrderId = reader.GetInt32("OrderId"),
                    ProductVariantId = reader.GetInt32("ProductVariantId"),
                    Quantity = reader.GetInt32("Quantity"),
                    PriceAtTime = reader.GetDecimal("PriceAtTime")
                };
            }
            return null;
        }

        public async Task<IEnumerable<OrderItem>> GetAllAsync()
        {
            var orderItems = new List<OrderItem>();
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            var command = new SqlCommand("SELECT * FROM OrderItems", connection);

            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                orderItems.Add(new OrderItem
                {
                    Id = reader.GetInt32("Id"),
                    OrderId = reader.GetInt32("OrderId"),
                    ProductVariantId = reader.GetInt32("ProductVariantId"),
                    Quantity = reader.GetInt32("Quantity"),
                    PriceAtTime = reader.GetDecimal("PriceAtTime")
                });
            }
            return orderItems;
        }

        public async Task AddAsync(OrderItem entity)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            var command = new SqlCommand(
                "INSERT INTO OrderItems (OrderId, ProductVariantId, Quantity, PriceAtTime) " +
                "VALUES (@OrderId, @ProductVariantId, @Quantity, @PriceAtTime)", connection);
            command.Parameters.AddWithValue("@OrderId", entity.OrderId);
            command.Parameters.AddWithValue("@ProductVariantId", entity.ProductVariantId);
            command.Parameters.AddWithValue("@Quantity", entity.Quantity);
            command.Parameters.AddWithValue("@PriceAtTime", entity.PriceAtTime);

            await command.ExecuteNonQueryAsync();
        }

        public async Task UpdateAsync(OrderItem entity)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            var command = new SqlCommand(
                "UPDATE OrderItems SET OrderId = @OrderId, ProductVariantId = @ProductVariantId, " +
                "Quantity = @Quantity, PriceAtTime = @PriceAtTime WHERE Id = @Id", connection);
            command.Parameters.AddWithValue("@Id", entity.Id);
            command.Parameters.AddWithValue("@OrderId", entity.OrderId);
            command.Parameters.AddWithValue("@ProductVariantId", entity.ProductVariantId);
            command.Parameters.AddWithValue("@Quantity", entity.Quantity);
            command.Parameters.AddWithValue("@PriceAtTime", entity.PriceAtTime);

            await command.ExecuteNonQueryAsync();
        }

        public async Task DeleteAsync(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            var command = new SqlCommand("DELETE FROM OrderItems WHERE Id = @Id", connection);
            command.Parameters.AddWithValue("@Id", id);

            await command.ExecuteNonQueryAsync();
        }
    }
}