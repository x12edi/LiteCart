namespace ECommerce.Application.Dtos
{
    public class ProductCategoriesDto
    {
        public int ProductId { get; set; }
        public int CategoryId { get; set; }
    }

    public class CreateProductCategoriesDto
    {
        public int ProductId { get; set; }
        public int CategoryId { get; set; }
    }
}