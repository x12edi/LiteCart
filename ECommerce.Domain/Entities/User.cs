namespace ECommerce.Domain.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; } // Unique in database
        public string PasswordHash { get; set; }
        public string Phone { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsActive { get; set; }
        public bool IsCustomer { get; set; }

        public ICollection<UserRole> Roles { get; set; } = new List<UserRole>();
    }
}