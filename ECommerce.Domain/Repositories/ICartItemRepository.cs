using ECommerce.Domain.Entities;

namespace ECommerce.Domain.Repositories
{
    public interface ICartItemRepository : IRepository<CartItem>
    {
        Task<CartItem> GetByCartAndProductVariantIdAsync(int cartId, int productVariantId);
        Task DeleteByCartAndProductVariantIdAsync(int cartId, int productVariantId);
    }
}