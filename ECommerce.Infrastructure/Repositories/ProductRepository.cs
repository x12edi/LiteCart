using ECommerce.Domain.Entities;
using ECommerce.Domain.Repositories;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace ECommerce.Infrastructure.Repositories
{
    public class ProductRepository : IRepository<Product>
    {
        private readonly string _connectionString;

        public ProductRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<Product> GetByIdAsync(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            var command = new SqlCommand("SELECT * FROM Products WHERE Id = @Id", connection);
            command.Parameters.AddWithValue("@Id", id);

            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new Product
                {
                    Id = reader.GetInt32("Id"),
                    Name = reader.GetString("Name"),
                    Description = reader.GetString("Description"),
                    Price = reader.GetDecimal("Price"),
                    SKU = reader.GetString("SKU"),
                    Status = reader.GetString("Status"),
                    Images = reader["Images"] != DBNull.Value ? (byte[])reader["Images"] : null,
                    CreatedAt = reader.GetDateTime("CreatedAt"),
                    UpdatedAt = reader.GetDateTime("UpdatedAt")
                };
            }
            return null;
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            var products = new List<Product>();
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            var command = new SqlCommand("SELECT * FROM Products", connection);

            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                products.Add(new Product
                {
                    Id = reader.GetInt32("Id"),
                    Name = reader.GetString("Name"),
                    Description = reader.GetString("Description"),
                    Price = reader.GetDecimal("Price"),
                    SKU = reader.GetString("SKU"),
                    Status = reader.GetString("Status"),
                    Images = reader["Images"] != DBNull.Value ? (byte[])reader["Images"] : null,
                    CreatedAt = reader.GetDateTime("CreatedAt"),
                    UpdatedAt = reader.GetDateTime("UpdatedAt")
                });
            }
            return products;
        }

        public async Task AddAsync(Product entity)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            var command = new SqlCommand(
                "INSERT INTO Products (Name, Description, Price, SKU, Status, Images, CreatedAt, UpdatedAt) " +
                "OUTPUT INSERTED.Id " +
                "VALUES (@Name, @Description, @Price, @SKU, @Status, @Images, GETUTCDATE(), GETUTCDATE())", connection);
            command.Parameters.AddWithValue("@Name", entity.Name);
            command.Parameters.AddWithValue("@Description", (object)entity.Description ?? DBNull.Value);
            command.Parameters.AddWithValue("@Price", entity.Price);
            command.Parameters.AddWithValue("@SKU", entity.SKU);
            command.Parameters.AddWithValue("@Status", entity.Status);
            command.Parameters.AddWithValue("@Images", (object)entity.Images ?? DBNull.Value);

            entity.Id = (int)await command.ExecuteScalarAsync();
        }

        public async Task UpdateAsync(Product entity)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            var command = new SqlCommand(
                "UPDATE Products SET Name = @Name, Description = @Description, Price = @Price, " +
                "SKU = @SKU, Status = @Status, Images = @Images, UpdatedAt = GETUTCDATE() WHERE Id = @Id", connection);
            command.Parameters.AddWithValue("@Id", entity.Id);
            command.Parameters.AddWithValue("@Name", entity.Name);
            command.Parameters.AddWithValue("@Description", (object)entity.Description ?? DBNull.Value);
            command.Parameters.AddWithValue("@Price", entity.Price);
            command.Parameters.AddWithValue("@SKU", entity.SKU);
            command.Parameters.AddWithValue("@Status", entity.Status);
            command.Parameters.AddWithValue("@Images", (object)entity.Images ?? DBNull.Value);

            await command.ExecuteNonQueryAsync();
        }

        public async Task DeleteAsync(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            var command = new SqlCommand("DELETE FROM Products WHERE Id = @Id", connection);
            command.Parameters.AddWithValue("@Id", id);

            await command.ExecuteNonQueryAsync();
        }
    }
}