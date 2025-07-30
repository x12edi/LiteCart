using ECommerce.Application.Dtos;

namespace ECommerce.Application.Services
{
    public interface IAdminLogService
    {
        Task<AdminLogDto> GetByIdAsync(int id);
        Task<IEnumerable<AdminLogDto>> GetAllAsync();
        Task<AdminLogDto> CreateAsync(CreateAdminLogDto dto);
        Task DeleteAsync(int id);
    }
}