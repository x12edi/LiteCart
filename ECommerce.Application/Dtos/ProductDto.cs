namespace ECommerce.Application.Dtos
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal BasePrice { get; set; }
        public bool IsActive { get; set; }
        public IEnumerable<int> CategoryIds { get; set; }
    }

    public class CreateProductDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal BasePrice { get; set; }
        public bool IsActive { get; set; }
        public IEnumerable<int> CategoryIds { get; set; }
    }

    public class UpdateProductDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal BasePrice { get; set; }
        public bool IsActive { get; set; }
        public IEnumerable<int> CategoryIds { get; set; }
    }
}