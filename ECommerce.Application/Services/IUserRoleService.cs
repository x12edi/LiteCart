using ECommerce.Application.Dtos;

namespace ECommerce.Application.Services
{
    public interface IUserRoleService
    {
        Task<UserRoleDto> GetByKeysAsync(int userId, int roleId);
        Task<IEnumerable<UserRoleDto>> GetByUserIdAsync(int userId);
        Task<IEnumerable<UserRoleDto>> GetAllAsync();
        Task<UserRoleDto> CreateAsync(CreateUserRoleDto dto);
        Task DeleteAsync(int userId, int roleId);
    }
}