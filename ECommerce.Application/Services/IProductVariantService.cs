using ECommerce.Application.Dtos;

namespace ECommerce.Application.Services
{
    public interface IProductVariantService
    {
        Task<ProductVariantDto> GetByIdAsync(int id);
        Task<IEnumerable<ProductVariantDto>> GetAllAsync();
        Task<ProductVariantDto> CreateAsync(CreateProductVariantDto dto);
        Task UpdateAsync(int id, UpdateProductVariantDto dto);
        Task DeleteAsync(int id);
    }
}