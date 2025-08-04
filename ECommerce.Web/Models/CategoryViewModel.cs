using System.ComponentModel.DataAnnotations;

namespace ECommerce.Web.Models
{
    public class CategoryViewModel
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; } // For display

        public int? ParentId { get; set; }

        public string? ParentCategory { get; set; }
        public string? SuccessMessage { get; set; }
        public string? ErrorMessage { get; set; }
    }
}
