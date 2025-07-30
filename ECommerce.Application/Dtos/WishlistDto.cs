namespace ECommerce.Application.Dtos
{
    public class WishlistDto
    {
        public int UserId { get; set; }
        public int ProductId { get; set; }
    }

    public class CreateWishlistDto
    {
        public int UserId { get; set; }
        public int ProductId { get; set; }
    }
}