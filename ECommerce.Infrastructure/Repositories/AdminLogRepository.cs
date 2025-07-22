using ECommerce.Domain.Entities;
using ECommerce.Domain.Repositories;
using Microsoft.Data.SqlClient;
using System.Data;

namespace ECommerce.Infrastructure.Repositories
{
    public class AdminLogRepository : IRepository<AdminLog>
    {
        private readonly string _connectionString;

        public AdminLogRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<AdminLog> GetByIdAsync(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            var command = new SqlCommand("SELECT * FROM AdminLogs WHERE Id = @Id", connection);
            command.Parameters.AddWithValue("@Id", id);

            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new AdminLog
                {
                    Id = reader.GetInt32("Id"),
                    UserId = reader.GetInt32("UserId"),
                    Action = reader.GetString("Action"),
                    Entity = reader.GetString("Entity"),
                    EntityId = reader.GetInt32("EntityId"),
                    Timestamp = reader.GetDateTime("Timestamp")
                };
            }
            return null;
        }

        public async Task<IEnumerable<AdminLog>> GetAllAsync()
        {
            var adminLogs = new List<AdminLog>();
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            var command = new SqlCommand("SELECT * FROM AdminLogs", connection);

            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                adminLogs.Add(new AdminLog
                {
                    Id = reader.GetInt32("Id"),
                    UserId = reader.GetInt32("UserId"),
                    Action = reader.GetString("Action"),
                    Entity = reader.GetString("Entity"),
                    EntityId = reader.GetInt32("EntityId"),
                    Timestamp = reader.GetDateTime("Timestamp")
                });
            }
            return adminLogs;
        }

        public async Task AddAsync(AdminLog entity)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            var command = new SqlCommand(
                "INSERT INTO AdminLogs (UserId, Action, Entity, EntityId, Timestamp) " +
                "VALUES (@UserId, @Action, @Entity, @EntityId, @Timestamp)", connection);
            command.Parameters.AddWithValue("@UserId", entity.UserId);
            command.Parameters.AddWithValue("@Action", entity.Action);
            command.Parameters.AddWithValue("@Entity", entity.Entity);
            command.Parameters.AddWithValue("@EntityId", entity.EntityId);
            command.Parameters.AddWithValue("@Timestamp", entity.Timestamp);

            await command.ExecuteNonQueryAsync();
        }

        public async Task UpdateAsync(AdminLog entity)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            var command = new SqlCommand(
                "UPDATE AdminLogs SET UserId = @UserId, Action = @Action, Entity = @Entity, " +
                "EntityId = @EntityId, Timestamp = @Timestamp WHERE Id = @Id", connection);
            command.Parameters.AddWithValue("@Id", entity.Id);
            command.Parameters.AddWithValue("@UserId", entity.UserId);
            command.Parameters.AddWithValue("@Action", entity.Action);
            command.Parameters.AddWithValue("@Entity", entity.Entity);
            command.Parameters.AddWithValue("@EntityId", entity.EntityId);
            command.Parameters.AddWithValue("@Timestamp", entity.Timestamp);

            await command.ExecuteNonQueryAsync();
        }

        public async Task DeleteAsync(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            var command = new SqlCommand("DELETE FROM AdminLogs WHERE Id = @Id", connection);
            command.Parameters.AddWithValue("@Id", id);

            await command.ExecuteNonQueryAsync();
        }
    }
}