using ECommerce.Domain.Entities;
using ECommerce.Domain.Repositories;
using Microsoft.Data.SqlClient;
using System.Data;

namespace ECommerce.Infrastructure.Repositories
{
    public class NotificationRepository : IRepository<Notification>
    {
        private readonly string _connectionString;

        public NotificationRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<Notification> GetByIdAsync(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            var command = new SqlCommand("SELECT * FROM Notifications WHERE Id = @Id", connection);
            command.Parameters.AddWithValue("@Id", id);

            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new Notification
                {
                    Id = reader.GetInt32("Id"),
                    UserId = reader.GetInt32("UserId"),
                    Type = reader.GetString("Type"),
                    Message = reader.GetString("Message"),
                    Status = reader.GetString("Status"),
                    CreatedAt = reader.GetDateTime("CreatedAt")
                };
            }
            return null;
        }

        public async Task<IEnumerable<Notification>> GetAllAsync()
        {
            var notifications = new List<Notification>();
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            var command = new SqlCommand("SELECT * FROM Notifications", connection);

            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                notifications.Add(new Notification
                {
                    Id = reader.GetInt32("Id"),
                    UserId = reader.GetInt32("UserId"),
                    Type = reader.GetString("Type"),
                    Message = reader.GetString("Message"),
                    Status = reader.GetString("Status"),
                    CreatedAt = reader.GetDateTime("CreatedAt")
                });
            }
            return notifications;
        }

        public async Task AddAsync(Notification entity)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            var command = new SqlCommand(
                "INSERT INTO Notifications (UserId, Type, Message, Status, CreatedAt) " +
                "VALUES (@UserId, @Type, @Message, @Status, @CreatedAt)", connection);
            command.Parameters.AddWithValue("@UserId", entity.UserId);
            command.Parameters.AddWithValue("@Type", entity.Type);
            command.Parameters.AddWithValue("@Message", entity.Message);
            command.Parameters.AddWithValue("@Status", entity.Status);
            command.Parameters.AddWithValue("@CreatedAt", entity.CreatedAt);

            await command.ExecuteNonQueryAsync();
        }

        public async Task UpdateAsync(Notification entity)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            var command = new SqlCommand(
                "UPDATE Notifications SET UserId = @UserId, Type = @Type, Message = @Message, " +
                "Status = @Status, CreatedAt = @CreatedAt WHERE Id = @Id", connection);
            command.Parameters.AddWithValue("@Id", entity.Id);
            command.Parameters.AddWithValue("@UserId", entity.UserId);
            command.Parameters.AddWithValue("@Type", entity.Type);
            command.Parameters.AddWithValue("@Message", entity.Message);
            command.Parameters.AddWithValue("@Status", entity.Status);
            command.Parameters.AddWithValue("@CreatedAt", entity.CreatedAt);

            await command.ExecuteNonQueryAsync();
        }

        public async Task DeleteAsync(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            var command = new SqlCommand("DELETE FROM Notifications WHERE Id = @Id", connection);
            command.Parameters.AddWithValue("@Id", id);

            await command.ExecuteNonQueryAsync();
        }
    }
}