namespace ECommerce.Domain.Entities
{
    public class Permission
    {
        public int Id { get; set; }
        public string Action { get; set; } // e.g., Create, Read, Update, Delete
        public string Resource { get; set; } // e.g., Product, Order
        public ICollection<RolePermission> Roles { get; set; } = new List<RolePermission>();
    }
}