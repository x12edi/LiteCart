using ECommerce.Application.Dtos;

namespace ECommerce.Application.Services
{
    public interface IPermissionService
    {
        Task<PermissionDto> GetByIdAsync(int id);
        Task<IEnumerable<PermissionDto>> GetAllAsync();
        Task<PermissionDto> CreateAsync(CreatePermissionDto dto);
        Task UpdateAsync(int id, UpdatePermissionDto dto);
        Task DeleteAsync(int id);
    }
}