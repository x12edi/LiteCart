namespace ECommerce.Application.Dtos
{
    public class UserDto
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public string Phone { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsActive { get; set; }
        public bool IsCustomer { get; set; }
    }

    public class CreateUserDto
    {
        public string Email { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public string Phone { get; set; }
        public bool IsActive { get; set; }
        public bool IsCustomer { get; set; }
    }

    public class UpdateUserDto
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string Phone { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsActive { get; set; }
        public bool IsCustomer { get; set; }
    }
}