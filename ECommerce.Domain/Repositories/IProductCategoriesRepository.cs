using ECommerce.Domain.Entities;

namespace ECommerce.Domain.Repositories
{
    public interface IProductCategoriesRepository : IRepository<ProductCategories>
    {
        Task<ProductCategories> GetByProductAndCategoryIdAsync(int productId, int categoryId);
        Task DeleteByProductAndCategoryIdAsync(int productId, int categoryId);
        Task DeleteByCategoryIdAsync(int categoryId);
        Task DeleteByProductIdAsync(int productId);
        Task<IEnumerable<ProductCategories>> GetByProductIdAsync(int productId);
        
    }
}