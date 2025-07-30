namespace ECommerce.Application.Dtos
{
    public class DiscountDto
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public decimal Percentage { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidUntil { get; set; }
        public int? MaxUses { get; set; }
        public int Uses { get; set; }
    }

    public class CreateDiscountDto
    {
        public string Code { get; set; }
        public decimal Percentage { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidUntil { get; set; }
        public int? MaxUses { get; set; }
    }

    public class UpdateDiscountDto
    {
        public string Code { get; set; }
        public decimal Percentage { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidUntil { get; set; }
        public int? MaxUses { get; set; }
        public int Uses { get; set; }
    }
}