namespace ECommerce.Domain.Entities
{
    public class Payment
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public Order Order { get; set; }
        public decimal Amount { get; set; }
        public string Method { get; set; } // e.g., CreditCard, PayPal
        public string Status { get; set; } // e.g., Pending, Completed, Failed
        public string TransactionId { get; set; }
    }
}