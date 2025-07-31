using System.ComponentModel.DataAnnotations;

namespace ECommerce.Web.Models
{
    public class OrderViewModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string UserEmail { get; set; } // For display

        [Required(ErrorMessage = "Total amount is required")]
        [Range(0.01, 1000000, ErrorMessage = "Total amount must be between 0.01 and 1,000,000")]
        public decimal TotalAmount { get; set; }

        [Required(ErrorMessage = "Status is required")]
        [RegularExpression("^(Pending|Paid|Shipped)$", ErrorMessage = "Status must be Pending, Paid, or Shipped")]
        public string Status { get; set; }

        public DateTime CreatedAt { get; set; }
        public string SuccessMessage { get; set; }
        public string ErrorMessage { get; set; }
    }
}