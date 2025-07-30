namespace ECommerce.Application.Dtos
{
    public class RolePermissionDto
    {
        public int RoleId { get; set; }
        public int PermissionId { get; set; }
    }

    public class CreateRolePermissionDto
    {
        public int RoleId { get; set; }
        public int PermissionId { get; set; }
    }
}