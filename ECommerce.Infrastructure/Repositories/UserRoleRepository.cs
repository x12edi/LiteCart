using ECommerce.Domain.Entities;
using ECommerce.Domain.Repositories;
using Microsoft.Data.SqlClient;
using System.Data;

namespace ECommerce.Infrastructure.Repositories
{
    public class UserRoleRepository : IRepository<UserRole>
    {
        private readonly string _connectionString;

        public UserRoleRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<UserRole> GetByIdAsync(int id)
        {
            throw new NotSupportedException("GetById is not supported for UserRole. Use GetByUserAndRoleIdAsync instead.");
        }

        public async Task<UserRole> GetByUserAndRoleIdAsync(int userId, int roleId)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            var command = new SqlCommand(
                "SELECT * FROM UserRoles WHERE UserId = @UserId AND RoleId = @RoleId", connection);
            command.Parameters.AddWithValue("@UserId", userId);
            command.Parameters.AddWithValue("@RoleId", roleId);

            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new UserRole
                {
                    UserId = reader.GetInt32("UserId"),
                    RoleId = reader.GetInt32("RoleId")
                };
            }
            return null;
        }

        public async Task<IEnumerable<UserRole>> GetAllAsync()
        {
            var userRoles = new List<UserRole>();
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            var command = new SqlCommand("SELECT * FROM UserRoles", connection);

            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                userRoles.Add(new UserRole
                {
                    UserId = reader.GetInt32("UserId"),
                    RoleId = reader.GetInt32("RoleId")
                });
            }
            return userRoles;
        }

        public async Task AddAsync(UserRole entity)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            var command = new SqlCommand(
                "INSERT INTO UserRoles (UserId, RoleId) VALUES (@UserId, @RoleId)", connection);
            command.Parameters.AddWithValue("@UserId", entity.UserId);
            command.Parameters.AddWithValue("@RoleId", entity.RoleId);

            await command.ExecuteNonQueryAsync();
        }

        public async Task UpdateAsync(UserRole entity)
        {
            throw new NotSupportedException("Update is not supported for UserRole. Delete and re-add instead.");
        }

        public async Task DeleteAsync(int id)
        {
            throw new NotSupportedException("Delete by single Id is not supported for UserRole. Use DeleteByUserAndRoleIdAsync instead.");
        }

        public async Task DeleteByUserAndRoleIdAsync(int userId, int roleId)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            var command = new SqlCommand(
                "DELETE FROM UserRoles WHERE UserId = @UserId AND RoleId = @RoleId", connection);
            command.Parameters.AddWithValue("@UserId", userId);
            command.Parameters.AddWithValue("@RoleId", roleId);

            await command.ExecuteNonQueryAsync();
        }
    }
}