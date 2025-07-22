namespace ECommerce.Domain.Entities
{
    public class ReturnRequest
    {
        public int Id { get; set; }
        public int OrderItemId { get; set; }
        public OrderItem OrderItem { get; set; }
        public string Reason { get; set; }
        public string Status { get; set; } // e.g., Requested, Approved, Denied
        public string ResolutionType { get; set; } // e.g., Refund, Replacement
    }
}