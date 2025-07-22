namespace ECommerce.Domain.Entities
{
    public class Discount
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string DiscountType { get; set; } // e.g., Percentage, Fixed
        public decimal Value { get; set; }
        public DateTime? Expiration { get; set; }
        public int? UsageLimit { get; set; } // Nullable for unlimited
        public int? ApplicableProductId { get; set; } // Nullable for product-specific discounts
        public Product ApplicableProduct { get; set; }
        public int? ApplicableCategoryId { get; set; } // Nullable for category-specific discounts
        public Category ApplicableCategory { get; set; }
    }
}