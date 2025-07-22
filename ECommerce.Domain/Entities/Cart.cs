namespace ECommerce.Domain.Entities
{
    public class Cart
    {
        public int Id { get; set; }
        public int? UserId { get; set; } // Nullable for guest users
        public User User { get; set; }
        public string SessionId { get; set; } // For guest carts
        public ICollection<CartItem> Items { get; set; } = new List<CartItem>();
    }
}