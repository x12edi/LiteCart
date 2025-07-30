namespace ECommerce.Application.Dtos
{
    public class NotificationDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Type { get; set; }
        public string Message { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class CreateNotificationDto
    {
        public int UserId { get; set; }
        public string Type { get; set; }
        public string Message { get; set; }
        public string Status { get; set; }
    }

    public class UpdateNotificationDto
    {
        public string Status { get; set; }
    }
}