using ECommerce.Domain.Entities;

namespace ECommerce.Domain.Repositories
{
    public interface IRolePermissionRepository : IRepository<RolePermission>
    {
        Task<RolePermission> GetByRoleAndPermissionIdAsync(int roleId, int permissionId);
        Task DeleteByRoleAndPermissionIdAsync(int roleId, int permissionId);
        Task DeleteByRoleIdAsync(int roleId);
    }
}