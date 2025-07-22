using ECommerce.Domain.Entities;
using ECommerce.Domain.Repositories;
using Microsoft.Data.SqlClient;
using System.Data;

namespace ECommerce.Infrastructure.Repositories
{
    public class WishlistRepository : IRepository<Wishlist>
    {
        private readonly string _connectionString;

        public WishlistRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<Wishlist> GetByIdAsync(int id)
        {
            throw new NotSupportedException("GetById is not supported for Wishlist. Use GetByUserAndProductIdAsync instead.");
        }

        public async Task<Wishlist> GetByUserAndProductIdAsync(int userId, int productId)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            var command = new SqlCommand(
                "SELECT * FROM Wishlists WHERE UserId = @UserId AND ProductId = @ProductId", connection);
            command.Parameters.AddWithValue("@UserId", userId);
            command.Parameters.AddWithValue("@ProductId", productId);

            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new Wishlist
                {
                    UserId = reader.GetInt32("UserId"),
                    ProductId = reader.GetInt32("ProductId")
                };
            }
            return null;
        }

        public async Task<IEnumerable<Wishlist>> GetAllAsync()
        {
            var wishlists = new List<Wishlist>();
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            var command = new SqlCommand("SELECT * FROM Wishlists", connection);

            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                wishlists.Add(new Wishlist
                {
                    UserId = reader.GetInt32("UserId"),
                    ProductId = reader.GetInt32("ProductId")
                });
            }
            return wishlists;
        }

        public async Task AddAsync(Wishlist entity)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            var command = new SqlCommand(
                "INSERT INTO Wishlists (UserId, ProductId) VALUES (@UserId, @ProductId)", connection);
            command.Parameters.AddWithValue("@UserId", entity.UserId);
            command.Parameters.AddWithValue("@ProductId", entity.ProductId);

            await command.ExecuteNonQueryAsync();
        }

        public async Task UpdateAsync(Wishlist entity)
        {
            throw new NotSupportedException("Update is not supported for Wishlist. Delete and re-add instead.");
        }

        public async Task DeleteAsync(int id)
        {
            throw new NotSupportedException("Delete by single Id is not supported for Wishlist. Use DeleteByUserAndProductIdAsync instead.");
        }

        public async Task DeleteByUserAndProductIdAsync(int userId, int productId)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            var command = new SqlCommand(
                "DELETE FROM Wishlists WHERE UserId = @UserId AND ProductId = @ProductId", connection);
            command.Parameters.AddWithValue("@UserId", userId);
            command.Parameters.AddWithValue("@ProductId", productId);

            await command.ExecuteNonQueryAsync();
        }
    }
}