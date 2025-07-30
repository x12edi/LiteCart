using ECommerce.Application.Dtos;

namespace ECommerce.Application.Services
{
    public interface IReviewService
    {
        Task<ReviewDto> GetByKeysAsync(int productId, int userId);
        Task<IEnumerable<ReviewDto>> GetAllAsync();
        Task<ReviewDto> CreateAsync(CreateReviewDto dto);
        Task UpdateAsync(int productId, int userId, UpdateReviewDto dto);
        Task DeleteAsync(int productId, int userId);
    }
}