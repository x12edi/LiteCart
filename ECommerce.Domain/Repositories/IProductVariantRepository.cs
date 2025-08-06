using ECommerce.Domain.Entities;

namespace ECommerce.Domain.Repositories
{
    public interface IProductVariantRepository : IRepository<ProductVariant>
    {
        Task DeleteByProductIdAsync(int productId);
        Task<IEnumerable<ProductVariant>> GetByProductIdAsync(int productId);
    }
}