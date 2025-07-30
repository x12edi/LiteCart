using ECommerce.Application.Dtos;

namespace ECommerce.Application.Services
{
    public interface IRoleService
    {
        Task<RoleDto> GetByIdAsync(int id);
        Task<IEnumerable<RoleDto>> GetAllAsync();
        Task<RoleDto> CreateAsync(CreateRoleDto dto);
        Task UpdateAsync(int id, UpdateRoleDto dto);
        Task DeleteAsync(int id);
    }
}