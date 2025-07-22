using ECommerce.Domain.Entities;
using ECommerce.Domain.Repositories;
using Microsoft.Data.SqlClient;
using System.Data;

namespace ECommerce.Infrastructure.Repositories
{
    public class RoleRepository : IRepository<Role>
    {
        private readonly string _connectionString;

        public RoleRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<Role> GetByIdAsync(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            var command = new SqlCommand("SELECT * FROM Roles WHERE Id = @Id", connection);
            command.Parameters.AddWithValue("@Id", id);

            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new Role
                {
                    Id = reader.GetInt32("Id"),
                    Name = reader.GetString("Name")
                };
            }
            return null;
        }

        public async Task<IEnumerable<Role>> GetAllAsync()
        {
            var roles = new List<Role>();
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            var command = new SqlCommand("SELECT * FROM Roles", connection);

            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                roles.Add(new Role
                {
                    Id = reader.GetInt32("Id"),
                    Name = reader.GetString("Name")
                });
            }
            return roles;
        }

        public async Task AddAsync(Role entity)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            var command = new SqlCommand(
                "INSERT INTO Roles (Name) VALUES (@Name)", connection);
            command.Parameters.AddWithValue("@Name", entity.Name);

            await command.ExecuteNonQueryAsync();
        }

        public async Task UpdateAsync(Role entity)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            var command = new SqlCommand(
                "UPDATE Roles SET Name = @Name WHERE Id = @Id", connection);
            command.Parameters.AddWithValue("@Id", entity.Id);
            command.Parameters.AddWithValue("@Name", entity.Name);

            await command.ExecuteNonQueryAsync();
        }

        public async Task DeleteAsync(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            var command = new SqlCommand("DELETE FROM Roles WHERE Id = @Id", connection);
            command.Parameters.AddWithValue("@Id", id);

            await command.ExecuteNonQueryAsync();
        }
    }
}