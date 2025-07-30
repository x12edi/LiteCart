using ECommerce.Domain.Entities;

namespace ECommerce.Domain.Repositories
{
    public interface IInventoryRepository : IRepository<Inventory>
    {
        Task<Inventory> GetByProductVariantAndWarehouseIdAsync(int productId, int? variantId, int warehouseId);
        Task DeleteByProductVariantAndWarehouseIdAsync(int productId, int? variantId, int warehouseId);
        
    }
}