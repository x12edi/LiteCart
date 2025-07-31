using ECommerce.Domain.Entities;

namespace ECommerce.Domain.Repositories
{
    public interface IUserRoleRepository : IRepository<UserRole>
    {
        Task<IEnumerable<UserRole>> GetByUserIdAsync(int userId);
        Task<UserRole> GetByUserAndRoleIdAsync(int userId, int roleId);
        Task DeleteByUserAndRoleIdAsync(int userId, int roleId);

    }
}