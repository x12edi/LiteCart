using ECommerce.Application.Dtos;

namespace ECommerce.Application.Services
{
    public interface IAddressService
    {
        Task<AddressDto> GetByIdAsync(int id);
        Task<IEnumerable<AddressDto>> GetAllAsync();
        Task<AddressDto> CreateAsync(CreateAddressDto dto);
        Task UpdateAsync(int id, UpdateAddressDto dto);
        Task DeleteAsync(int id);
    }
}