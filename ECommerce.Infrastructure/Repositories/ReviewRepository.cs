using ECommerce.Domain.Entities;
using ECommerce.Domain.Repositories;
using Microsoft.Data.SqlClient;
using System.Data;

namespace ECommerce.Infrastructure.Repositories
{
    public class ReviewRepository : IRepository<Review>
    {
        private readonly string _connectionString;

        public ReviewRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<Review> GetByIdAsync(int id)
        {
            throw new NotSupportedException("GetById is not supported for Review. Use GetByProductAndUserIdAsync instead.");
        }

        public async Task<Review> GetByProductAndUserIdAsync(int productId, int userId)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            var command = new SqlCommand(
                "SELECT * FROM Reviews WHERE ProductId = @ProductId AND UserId = @UserId", connection);
            command.Parameters.AddWithValue("@ProductId", productId);
            command.Parameters.AddWithValue("@UserId", userId);

            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new Review
                {
                    ProductId = reader.GetInt32("ProductId"),
                    UserId = reader.GetInt32("UserId"),
                    Rating = reader.GetInt32("Rating"),
                    Comment = reader["Comment"] != DBNull.Value ? reader.GetString("Comment") : null,
                    CreatedAt = reader.GetDateTime("CreatedAt")
                };
            }
            return null;
        }

        public async Task<IEnumerable<Review>> GetAllAsync()
        {
            var reviews = new List<Review>();
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            var command = new SqlCommand("SELECT * FROM Reviews", connection);

            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                reviews.Add(new Review
                {
                    ProductId = reader.GetInt32("ProductId"),
                    UserId = reader.GetInt32("UserId"),
                    Rating = reader.GetInt32("Rating"),
                    Comment = reader["Comment"] != DBNull.Value ? reader.GetString("Comment") : null,
                    CreatedAt = reader.GetDateTime("CreatedAt")
                });
            }
            return reviews;
        }

        public async Task AddAsync(Review entity)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            var command = new SqlCommand(
                "INSERT INTO Reviews (ProductId, UserId, Rating, Comment, CreatedAt) " +
                "VALUES (@ProductId, @UserId, @Rating, @Comment, @CreatedAt)", connection);
            command.Parameters.AddWithValue("@ProductId", entity.ProductId);
            command.Parameters.AddWithValue("@UserId", entity.UserId);
            command.Parameters.AddWithValue("@Rating", entity.Rating);
            command.Parameters.AddWithValue("@Comment", (object)entity.Comment ?? DBNull.Value);
            command.Parameters.AddWithValue("@CreatedAt", entity.CreatedAt);

            await command.ExecuteNonQueryAsync();
        }

        public async Task UpdateAsync(Review entity)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            var command = new SqlCommand(
                "UPDATE Reviews SET Rating = @Rating, Comment = @Comment, CreatedAt = @CreatedAt " +
                "WHERE ProductId = @ProductId AND UserId = @UserId", connection);
            command.Parameters.AddWithValue("@ProductId", entity.ProductId);
            command.Parameters.AddWithValue("@UserId", entity.UserId);
            command.Parameters.AddWithValue("@Rating", entity.Rating);
            command.Parameters.AddWithValue("@Comment", (object)entity.Comment ?? DBNull.Value);
            command.Parameters.AddWithValue("@CreatedAt", entity.CreatedAt);

            await command.ExecuteNonQueryAsync();
        }

        public async Task DeleteAsync(int id)
        {
            throw new NotSupportedException("Delete by single Id is not supported for Review. Use DeleteByProductAndUserIdAsync instead.");
        }

        public async Task DeleteByProductAndUserIdAsync(int productId, int userId)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            var command = new SqlCommand(
                "DELETE FROM Reviews WHERE ProductId = @ProductId AND UserId = @UserId", connection);
            command.Parameters.AddWithValue("@ProductId", productId);
            command.Parameters.AddWithValue("@UserId", userId);

            await command.ExecuteNonQueryAsync();
        }
    }
}