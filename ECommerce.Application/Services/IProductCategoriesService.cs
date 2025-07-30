using ECommerce.Application.Dtos;

namespace ECommerce.Application.Services
{
    public interface IProductCategoriesService
    {
        Task<ProductCategoriesDto> GetByKeysAsync(int productId, int categoryId);
        Task<IEnumerable<ProductCategoriesDto>> GetAllAsync();
        Task<ProductCategoriesDto> CreateAsync(CreateProductCategoriesDto dto);
        Task DeleteAsync(int productId, int categoryId);
    }
}