using ECommerce.Application.Dtos;

namespace ECommerce.Application.Services
{
    public interface IRolePermissionService
    {
        Task<RolePermissionDto> GetByKeysAsync(int roleId, int permissionId);
        Task<IEnumerable<RolePermissionDto>> GetAllAsync();
        Task<RolePermissionDto> CreateAsync(CreateRolePermissionDto dto);
        Task DeleteAsync(int roleId, int permissionId);
    }
}