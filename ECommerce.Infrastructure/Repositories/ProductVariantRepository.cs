using ECommerce.Domain.Entities;
using ECommerce.Domain.Repositories;
using Microsoft.Data.SqlClient;
using System.Data;

namespace ECommerce.Infrastructure.Repositories
{
    public class ProductVariantRepository : IRepository<ProductVariant>
    {
        private readonly string _connectionString;

        public ProductVariantRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<ProductVariant> GetByIdAsync(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            var command = new SqlCommand("SELECT * FROM ProductVariants WHERE Id = @Id", connection);
            command.Parameters.AddWithValue("@Id", id);

            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new ProductVariant
                {
                    Id = reader.GetInt32("Id"),
                    ProductId = reader.GetInt32("ProductId"),
                    Size = reader["Size"] != DBNull.Value ? reader.GetString("Size") : null,
                    Color = reader["Color"] != DBNull.Value ? reader.GetString("Color") : null,
                    Price = reader.GetDecimal("Price"),
                    Stock = reader.GetInt32("Stock")
                };
            }
            return null;
        }

        public async Task<IEnumerable<ProductVariant>> GetAllAsync()
        {
            var variants = new List<ProductVariant>();
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            var command = new SqlCommand("SELECT * FROM ProductVariants", connection);

            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                variants.Add(new ProductVariant
                {
                    Id = reader.GetInt32("Id"),
                    ProductId = reader.GetInt32("ProductId"),
                    Size = reader["Size"] != DBNull.Value ? reader.GetString("Size") : null,
                    Color = reader["Color"] != DBNull.Value ? reader.GetString("Color") : null,
                    Price = reader.GetDecimal("Price"),
                    Stock = reader.GetInt32("Stock")
                });
            }
            return variants;
        }

        public async Task AddAsync(ProductVariant entity)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            var command = new SqlCommand(
                "INSERT INTO ProductVariants (ProductId, Size, Color, Price, Stock) " +
                "VALUES (@ProductId, @Size, @Color, @Price, @Stock)", connection);
            command.Parameters.AddWithValue("@ProductId", entity.ProductId);
            command.Parameters.AddWithValue("@Size", (object)entity.Size ?? DBNull.Value);
            command.Parameters.AddWithValue("@Color", (object)entity.Color ?? DBNull.Value);
            command.Parameters.AddWithValue("@Price", entity.Price);
            command.Parameters.AddWithValue("@Stock", entity.Stock);

            await command.ExecuteNonQueryAsync();
        }

        public async Task UpdateAsync(ProductVariant entity)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            var command = new SqlCommand(
                "UPDATE ProductVariants SET ProductId = @ProductId, Size = @Size, Color = @Color, " +
                "Price = @Price, Stock = @Stock WHERE Id = @Id", connection);
            command.Parameters.AddWithValue("@Id", entity.Id);
            command.Parameters.AddWithValue("@ProductId", entity.ProductId);
            command.Parameters.AddWithValue("@Size", (object)entity.Size ?? DBNull.Value);
            command.Parameters.AddWithValue("@Color", (object)entity.Color ?? DBNull.Value);
            command.Parameters.AddWithValue("@Price", entity.Price);
            command.Parameters.AddWithValue("@Stock", entity.Stock);

            await command.ExecuteNonQueryAsync();
        }

        public async Task DeleteAsync(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            var command = new SqlCommand("DELETE FROM ProductVariants WHERE Id = @Id", connection);
            command.Parameters.AddWithValue("@Id", id);

            await command.ExecuteNonQueryAsync();
        }
    }
}