namespace ECommerce.Application.Dtos
{
    public class PermissionDto
    {
        public int Id { get; set; }
        public string Action { get; set; }
        public string Resource { get; set; }
    }

    public class CreatePermissionDto
    {
        public string Action { get; set; }
        public string Resource { get; set; }
    }

    public class UpdatePermissionDto
    {
        public string Action { get; set; }
        public string Resource { get; set; }
    }
}