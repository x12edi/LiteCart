using ECommerce.Application.Dtos;

namespace ECommerce.Application.Services
{
    public interface IInventoryService
    {
        Task<InventoryDto> GetByKeysAsync(int productVariantId, int variantId, int warehouseId);
        Task<IEnumerable<InventoryDto>> GetAllAsync();
        Task<InventoryDto> CreateAsync(CreateInventoryDto dto);
        Task UpdateAsync(int productId, int productVariantId, int warehouseId, UpdateInventoryDto dto);
        Task DeleteAsync(int productId, int productVariantId, int warehouseId);
    }
}