namespace ECommerce.Application.Dtos
{
    public class ReviewDto
    {
        public int ProductId { get; set; }
        public int UserId { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class CreateReviewDto
    {
        public int ProductId { get; set; }
        public int UserId { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; }
    }

    public class UpdateReviewDto
    {
        public int Rating { get; set; }
        public string Comment { get; set; }
    }
}