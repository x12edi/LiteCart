using ECommerce.Application.Dtos;

namespace ECommerce.Application.Services
{
    public interface IReturnRequestService
    {
        Task<ReturnRequestDto> GetByIdAsync(int id);
        Task<IEnumerable<ReturnRequestDto>> GetAllAsync();
        Task<ReturnRequestDto> CreateAsync(CreateReturnRequestDto dto);
        Task UpdateAsync(int id, UpdateReturnRequestDto dto);
        Task DeleteAsync(int id);
    }
}