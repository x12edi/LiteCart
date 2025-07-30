namespace ECommerce.Application.Dtos
{
    public class RoleDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class CreateRoleDto
    {
        public string Name { get; set; }
    }

    public class UpdateRoleDto
    {
        public string Name { get; set; }
    }
}