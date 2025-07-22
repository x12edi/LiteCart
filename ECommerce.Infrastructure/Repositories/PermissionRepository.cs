using ECommerce.Domain.Entities;
using ECommerce.Domain.Repositories;
using Microsoft.Data.SqlClient;
using System.Data;

namespace ECommerce.Infrastructure.Repositories
{
    public class PermissionRepository : IRepository<Permission>
    {
        private readonly string _connectionString;

        public PermissionRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<Permission> GetByIdAsync(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            var command = new SqlCommand("SELECT * FROM Permissions WHERE Id = @Id", connection);
            command.Parameters.AddWithValue("@Id", id);

            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new Permission
                {
                    Id = reader.GetInt32("Id"),
                    Action = reader.GetString("Action"),
                    Resource = reader.GetString("Resource")
                };
            }
            return null;
        }

        public async Task<IEnumerable<Permission>> GetAllAsync()
        {
            var permissions = new List<Permission>();
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            var command = new SqlCommand("SELECT * FROM Permissions", connection);

            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                permissions.Add(new Permission
                {
                    Id = reader.GetInt32("Id"),
                    Action = reader.GetString("Action"),
                    Resource = reader.GetString("Resource")
                });
            }
            return permissions;
        }

        public async Task AddAsync(Permission entity)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            var command = new SqlCommand(
                "INSERT INTO Permissions (Action, Resource) VALUES (@Action, @Resource)", connection);
            command.Parameters.AddWithValue("@Action", entity.Action);
            command.Parameters.AddWithValue("@Resource", entity.Resource);

            await command.ExecuteNonQueryAsync();
        }

        public async Task UpdateAsync(Permission entity)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            var command = new SqlCommand(
                "UPDATE Permissions SET Action = @Action, Resource = @Resource WHERE Id = @Id", connection);
            command.Parameters.AddWithValue("@Id", entity.Id);
            command.Parameters.AddWithValue("@Action", entity.Action);
            command.Parameters.AddWithValue("@Resource", entity.Resource);

            await command.ExecuteNonQueryAsync();
        }

        public async Task DeleteAsync(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            var command = new SqlCommand("DELETE FROM Permissions WHERE Id = @Id", connection);
            command.Parameters.AddWithValue("@Id", id);

            await command.ExecuteNonQueryAsync();
        }
    }
}