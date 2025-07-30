using ECommerce.Application.Dtos;

namespace ECommerce.Application.Services
{
    public interface IPaymentService
    {
        Task<PaymentDto> GetByIdAsync(int id);
        Task<IEnumerable<PaymentDto>> GetAllAsync();
        Task<PaymentDto> CreateAsync(CreatePaymentDto dto);
        Task UpdateAsync(int id, UpdatePaymentDto dto);
        Task DeleteAsync(int id);
    }
}