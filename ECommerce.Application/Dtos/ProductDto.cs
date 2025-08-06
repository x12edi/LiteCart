namespace ECommerce.Application.Dtos
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string SKU { get; set; }
        public string Status { get; set; } // Active, Inactive, Discontinued
        public byte[] Images { get; set; } // VARBINARY(MAX)
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public List<int> CategoryIds { get; set; } = new List<int>(); // For ProductCategories
        public List<ProductVariantDto> Variants { get; set; } = new List<ProductVariantDto>();
    }

    public class CreateProductDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string SKU { get; set; }
        public string Status { get; set; } // Active, Inactive, Discontinued
        public byte[] Images { get; set; } // VARBINARY(MAX)
        public IEnumerable<int> CategoryIds { get; set; }

        public IEnumerable<ProductVariantDto> Variants { get; set; } 
    }

    public class UpdateProductDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string SKU { get; set; }
        public string Status { get; set; } // Active, Inactive, Discontinued
        public byte[] Images { get; set; } // VARBINARY(MAX)
        public IEnumerable<int> CategoryIds { get; set; }
        public IEnumerable<ProductVariantDto> Variants { get; set; }
    }
}