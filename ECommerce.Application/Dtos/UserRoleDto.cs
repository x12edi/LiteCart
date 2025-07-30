namespace ECommerce.Application.Dtos
{
    public class UserRoleDto
    {
        public int UserId { get; set; }
        public int RoleId { get; set; }
    }

    public class CreateUserRoleDto
    {
        public int UserId { get; set; }
        public int RoleId { get; set; }
    }
}