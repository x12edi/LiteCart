namespace ECommerce.Domain.Entities
{
    public class AdminLog
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public string Action { get; set; } // e.g., Created Product, Updated Order
        public string Entity { get; set; } // e.g., Product, Order
        public int EntityId { get; set; }
        public DateTime Timestamp { get; set; }
    }
}