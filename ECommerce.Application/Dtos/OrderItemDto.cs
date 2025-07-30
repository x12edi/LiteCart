namespace ECommerce.Application.Dtos
{
    public class OrderItemDto
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int ProductVariantId { get; set; }
        public int Quantity { get; set; }
        public decimal PriceAtTime { get; set; }
    }

    public class CreateOrderItemDto
    {
        public int ProductVariantId { get; set; }
        public int Quantity { get; set; }
    }
}