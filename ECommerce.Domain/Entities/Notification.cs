namespace ECommerce.Domain.Entities
{
    public class Notification
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public string Type { get; set; } // e.g., OrderConfirmation, ShippingUpdate
        public string Message { get; set; }
        public string Status { get; set; } // e.g., Pending, Sent, Failed
        public DateTime CreatedAt { get; set; }
    }
}