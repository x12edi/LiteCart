namespace ECommerce.Application.Dtos
{
    public class CartDto
    {
        public int Id { get; set; }
        public int? UserId { get; set; }
        public string SessionId { get; set; }
        public IEnumerable<CartItemDto> Items { get; set; }
    }

    public class CreateCartDto
    {
        public int? UserId { get; set; }
        public string SessionId { get; set; }
    }

    public class CartItemDto
    {
        public int CartId { get; set; }
        public int ProductVariantId { get; set; }
        public int Quantity { get; set; }
        public decimal PriceAtTime { get; set; }
    }

    public class AddCartItemDto
    {
        public int ProductVariantId { get; set; }
        public int Quantity { get; set; }
    }
}