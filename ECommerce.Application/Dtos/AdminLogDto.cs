namespace ECommerce.Application.Dtos
{
    public class AdminLogDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Action { get; set; }
        public string Entity { get; set; }
        public int EntityId { get; set; }
        public DateTime Timestamp { get; set; }
    }

    public class CreateAdminLogDto
    {
        public int UserId { get; set; }
        public string Action { get; set; }
        public string Entity { get; set; }
        public int EntityId { get; set; }
    }
}