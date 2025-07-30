using ECommerce.Application.Dtos;

namespace ECommerce.Application.Services
{
    public interface IOrderService
    {
        Task<OrderDto> GetByIdAsync(int id);
        Task<IEnumerable<OrderDto>> GetAllAsync();
        Task<OrderDto> CreateAsync(CreateOrderDto dto);
        Task UpdateAsync(int id, UpdateOrderDto dto);
        Task DeleteAsync(int id);
    }
}