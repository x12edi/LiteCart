using ECommerce.Application.Dtos;

namespace ECommerce.Application.Services
{
    public interface IStockMovementService
    {
        Task<StockMovementDto> GetByIdAsync(int id);
        Task<IEnumerable<StockMovementDto>> GetAllAsync();
        Task<StockMovementDto> CreateAsync(CreateStockMovementDto dto);
        Task DeleteAsync(int id);
    }
}