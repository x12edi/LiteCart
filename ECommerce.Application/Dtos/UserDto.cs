namespace ECommerce.Application.Dtos
{
    public class UserDto
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public bool IsActive { get; set; }
    }

    public class CreateUserDto
    {
        public string Email { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public bool IsActive { get; set; }
    }

    public class UpdateUserDto
    {
        public string Email { get; set; }
        public string Username { get; set; }
        public bool IsActive { get; set; }
    }
}