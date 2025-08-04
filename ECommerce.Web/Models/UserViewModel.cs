using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ECommerce.Web.Models
{
    public class UserViewModel : IValidatableObject
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Username is required")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        
        [DataType(DataType.Password)]
        public string? PasswordHash { get; set; }
        
        //[DataType(DataType.PhoneNumber)]
        [Phone(ErrorMessage = "Invalid phone number.")]
        public string? Phone { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsActive { get; set; }
        public bool IsCustomer { get; set; }
        public string? SuccessMessage { get; set; }
        public string? ErrorMessage { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Id == 0 && string.IsNullOrWhiteSpace(PasswordHash))
            {
                yield return new ValidationResult("Password is required.", new[] { nameof(PasswordHash) });
            }
        }
    }
}
