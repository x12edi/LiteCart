using ECommerce.Domain.Entities;
using ECommerce.Domain.Repositories;
using Microsoft.Data.SqlClient;
using System.Data;

namespace ECommerce.Infrastructure.Repositories
{
    public class RolePermissionRepository : IRepository<RolePermission>
    {
        private readonly string _connectionString;

        public RolePermissionRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<RolePermission> GetByIdAsync(int id)
        {
            throw new NotSupportedException("GetById is not supported for RolePermission. Use GetByRoleAndPermissionIdAsync instead.");
        }

        public async Task<RolePermission> GetByRoleAndPermissionIdAsync(int roleId, int permissionId)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            var command = new SqlCommand(
                "SELECT * FROM RolePermissions WHERE RoleId = @RoleId AND PermissionId = @PermissionId", connection);
            command.Parameters.AddWithValue("@RoleId", roleId);
            command.Parameters.AddWithValue("@PermissionId", permissionId);

            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new RolePermission
                {
                    RoleId = reader.GetInt32("RoleId"),
                    PermissionId = reader.GetInt32("PermissionId")
                };
            }
            return null;
        }

        public async Task<IEnumerable<RolePermission>> GetAllAsync()
        {
            var rolePermissions = new List<RolePermission>();
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            var command = new SqlCommand("SELECT * FROM RolePermissions", connection);

            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                rolePermissions.Add(new RolePermission
                {
                    RoleId = reader.GetInt32("RoleId"),
                    PermissionId = reader.GetInt32("PermissionId")
                });
            }
            return rolePermissions;
        }

        public async Task AddAsync(RolePermission entity)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            var command = new SqlCommand(
                "INSERT INTO RolePermissions (RoleId, PermissionId) VALUES (@RoleId, @PermissionId)", connection);
            command.Parameters.AddWithValue("@RoleId", entity.RoleId);
            command.Parameters.AddWithValue("@PermissionId", entity.PermissionId);

            await command.ExecuteNonQueryAsync();
        }

        public async Task UpdateAsync(RolePermission entity)
        {
            throw new NotSupportedException("Update is not supported for RolePermission. Delete and re-add instead.");
        }

        public async Task DeleteAsync(int id)
        {
            throw new NotSupportedException("Delete by single Id is not supported for RolePermission. Use DeleteByRoleAndPermissionIdAsync instead.");
        }

        public async Task DeleteByRoleAndPermissionIdAsync(int roleId, int permissionId)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            var command = new SqlCommand(
                "DELETE FROM RolePermissions WHERE RoleId = @RoleId AND PermissionId = @PermissionId", connection);
            command.Parameters.AddWithValue("@RoleId", roleId);
            command.Parameters.AddWithValue("@PermissionId", permissionId);

            await command.ExecuteNonQueryAsync();
        }
    }
}