using ECommerce.Application.Dtos;

namespace ECommerce.Application.Services
{
    public interface ICategoryService
    {
        Task<CategoryDto> GetByIdAsync(int id);
        Task<IEnumerable<CategoryDto>> GetAllAsync();
        Task<CategoryDto> CreateAsync(CreateCategoryDto dto);
        Task UpdateAsync(int id, UpdateCategoryDto dto);
        Task DeleteAsync(int id);
    }
}