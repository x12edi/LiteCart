using ECommerce.Application.Dtos;

namespace ECommerce.Application.Services
{
    public interface ICustomerSupportTicketService
    {
        Task<CustomerSupportTicketDto> GetByIdAsync(int id);
        Task<IEnumerable<CustomerSupportTicketDto>> GetAllAsync();
        Task<CustomerSupportTicketDto> CreateAsync(CreateCustomerSupportTicketDto dto);
        Task UpdateAsync(int id, UpdateCustomerSupportTicketDto dto);
        Task DeleteAsync(int id);
    }
}