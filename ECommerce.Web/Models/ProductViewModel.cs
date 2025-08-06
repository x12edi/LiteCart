using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace ECommerce.Web.Models
{
    public class ProductViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string Name { get; set; }

        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Price is required")]
        [Range(0.01, 10000, ErrorMessage = "Price must be between 0.01 and 10,000")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "SKU is required")]
        [StringLength(50, ErrorMessage = "SKU cannot exceed 50 characters")]
        public string SKU { get; set; }

        [Required(ErrorMessage = "Status is required")]
        [RegularExpression("^(Active|Inactive|Discontinued)$", ErrorMessage = "Status must be Active, Inactive, or Discontinued")]
        public string Status { get; set; }

        public IFormFile? ImageFile { get; set; } // For uploading images
        
        public string? ImageBase64 { get; set; } // For displaying images
        
        [ValidateNever]
        public DateTime CreatedAt { get; set; }
        [ValidateNever]
        public DateTime UpdatedAt { get; set; }

        [Required(ErrorMessage = "At least one category is required")]
        public List<int> CategoryIds { get; set; } = new List<int>();

        public List<ProductVariantViewModel> ProductVariants { get; set; } = new List<ProductVariantViewModel>();

        [ValidateNever]
        public string SuccessMessage { get; set; }
        [ValidateNever]
        public string ErrorMessage { get; set; }
    }

    public class ProductListViewModel
    {
        public List<ProductViewModel> Products { get; set; } = new List<ProductViewModel>();
        public List<CategoryViewModel> Categories { get; set; } = new List<CategoryViewModel>();
        public string SearchQuery { get; set; }
        public List<int> SelectedCategoryIds { get; set; } = new List<int>();
        public string SortOption { get; set; } // e.g., PriceAsc, PriceDesc, NameAsc, NameDesc, Newest
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 12;
        public int TotalProducts { get; set; }
    }

    public class ProductVariantViewModel
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string Size { get; set; }
        public string Color { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public string DisplayName => $"{(Size != null ? Size : "")}{(Size != null && Color != null ? " - " : "")}{(Color != null ? Color : "")}".Trim();
    }

}