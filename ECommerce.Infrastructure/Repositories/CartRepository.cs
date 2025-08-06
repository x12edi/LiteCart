using ECommerce.Domain.Entities;
using ECommerce.Domain.Repositories;
using Microsoft.Data.SqlClient;
using System.Data;

namespace ECommerce.Infrastructure.Repositories
{
    public class CartRepository : ICartRepository
    {
        private readonly string _connectionString;

        public CartRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<Cart> GetByIdAsync(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            var command = new SqlCommand("SELECT * FROM Carts WHERE Id = @Id", connection);
            command.Parameters.AddWithValue("@Id", id);

            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new Cart
                {
                    Id = reader.GetInt32("Id"),
                    UserId = reader["UserId"] != DBNull.Value ? reader.GetInt32("UserId") : null,
                    SessionId = reader.GetString("SessionId")
                };
            }
            return null;
        }

        public async Task<Cart> GetByUserIdAsync(string userId)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            var command = new SqlCommand("SELECT * FROM Carts WHERE UserId = @UserId", connection);
            command.Parameters.AddWithValue("@UserId", userId);

            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new Cart
                {
                    Id = reader.GetInt32("Id"),
                    UserId = reader.GetInt32("UserId"),
                    SessionId = reader.GetString("SessionId")
                };
            }
            return null;
        }


        public async Task<IEnumerable<Cart>> GetAllAsync()
        {
            var carts = new List<Cart>();
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            var command = new SqlCommand("SELECT * FROM Carts", connection);

            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                carts.Add(new Cart
                {
                    Id = reader.GetInt32("Id"),
                    UserId = reader["UserId"] != DBNull.Value ? reader.GetInt32("UserId") : null,
                    SessionId = reader.GetString("SessionId")
                });
            }
            return carts;
        }

        public async Task AddAsync(Cart entity)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            var command = new SqlCommand(
                "INSERT INTO Carts (UserId, SessionId) OUTPUT INSERTED.Id VALUES (@UserId, @SessionId)", connection);
            command.Parameters.AddWithValue("@UserId", entity.UserId);
            command.Parameters.AddWithValue("@SessionId", entity.SessionId);

            entity.Id = (int)await command.ExecuteScalarAsync();
        }


        public async Task UpdateAsync(Cart entity)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            var command = new SqlCommand(
                "UPDATE Carts SET UserId = @UserId, SessionId = @SessionId WHERE Id = @Id", connection);
            command.Parameters.AddWithValue("@Id", entity.Id);
            command.Parameters.AddWithValue("@UserId", (object)entity.UserId ?? DBNull.Value);
            command.Parameters.AddWithValue("@SessionId", entity.SessionId);

            await command.ExecuteNonQueryAsync();
        }

        public async Task DeleteAsync(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            var command = new SqlCommand("DELETE FROM Carts WHERE Id = @Id", connection);
            command.Parameters.AddWithValue("@Id", id);

            await command.ExecuteNonQueryAsync();
        }
    }
}