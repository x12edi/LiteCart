namespace ECommerce.Application.Dtos
{
    public class PaymentDto
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public decimal Amount { get; set; }
        public string Method { get; set; }
        public string Status { get; set; }
        public string TransactionId { get; set; }
    }

    public class CreatePaymentDto
    {
        public int OrderId { get; set; }
        public decimal Amount { get; set; }
        public string Method { get; set; }
        public string Status { get; set; }
        public string TransactionId { get; set; }
    }

    public class UpdatePaymentDto
    {
        public string Status { get; set; }
        public string TransactionId { get; set; }
    }
}