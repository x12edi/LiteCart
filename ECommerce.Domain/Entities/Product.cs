namespace ECommerce.Domain.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string SKU { get; set; }
        public string Status { get; set; } // e.g., Active, Inactive, Discontinued
        public byte[] Images { get; set; } // Binary data for storing images
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}