namespace ECommerce.Domain.Entities
{
    public class Role
    {
        public int Id { get; set; }
        public string Name { get; set; } // e.g., Admin, Customer, Manager
        public ICollection<UserRole> Users { get; set; } = new List<UserRole>();
        public ICollection<RolePermission> Permissions { get; set; } = new List<RolePermission>();
    }
}