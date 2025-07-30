using ECommerce.Application.Dtos;

namespace ECommerce.Application.Services
{
    public interface INotificationService
    {
        Task<NotificationDto> GetByIdAsync(int id);
        Task<IEnumerable<NotificationDto>> GetAllAsync();
        Task<NotificationDto> CreateAsync(CreateNotificationDto dto);
        Task UpdateAsync(int id, UpdateNotificationDto dto);
        Task DeleteAsync(int id);
    }
}