using ECommerce.Application.Dtos;

namespace ECommerce.Application.Services
{
    public interface IWishlistService
    {
        Task<WishlistDto> GetByKeysAsync(int userId, int productId);
        Task<IEnumerable<WishlistDto>> GetAllAsync();
        Task<WishlistDto> CreateAsync(CreateWishlistDto dto);
        Task DeleteAsync(int userId, int productId);
    }
}