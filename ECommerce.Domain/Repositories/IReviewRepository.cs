using ECommerce.Domain.Entities;

namespace ECommerce.Domain.Repositories
{
    public interface IReviewRepository : IRepository<Review>
    {
        Task<Review> GetByProductAndUserIdAsync(int productId, int userId);
        Task DeleteByProductAndUserIdAsync(int productId, int userId);
    }
}