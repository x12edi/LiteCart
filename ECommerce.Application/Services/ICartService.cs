using ECommerce.Application.Dtos;

namespace ECommerce.Application.Services
{
    public interface ICartService
    {
        Task<CartDto> GetByIdAsync(int id);
        Task<CartDto> GetByUserIdOrSessionAsync(int? userId, string sessionId);
        Task<CartDto> CreateAsync(CreateCartDto dto);
        Task AddItemAsync(CartDto cart, AddCartItemDto dto);
        Task UpdateItemAsync(int cartId, int productVariantId, int quantity);
        Task RemoveItemAsync(int cartId, int productVariantId);
        Task DeleteAsync(int id);
    }
}