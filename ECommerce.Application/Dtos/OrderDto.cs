
namespace ECommerce.Application.Dtos
{
    public class OrderDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; } // Pending, Paid, Shipped
        public DateTime CreatedAt { get; set; }
        public List<OrderItemDto> OrderItems { get; set; } = new List<OrderItemDto>();
    }

    public class CreateOrderDto
    {
        public int UserId { get; set; }
        public IEnumerable<CreateOrderItemDto> OrderItems { get; set; }
    }

    public class UpdateOrderDto
    {
        public string Status { get; set; }
    }
}