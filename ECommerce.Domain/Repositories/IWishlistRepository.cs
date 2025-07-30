using ECommerce.Domain.Entities;

namespace ECommerce.Domain.Repositories
{
    public interface IWishlistRepository : IRepository<Wishlist>
    {
        Task<Wishlist> GetByUserAndProductIdAsync(int userId, int productId);
        Task DeleteByUserAndProductIdAsync(int userId, int productId);
    }
}