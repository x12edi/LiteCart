namespace ECommerce.Domain.Entities
{
    public class TaxRule
    {
        public int Id { get; set; }
        public string Region { get; set; } // e.g., State, Country
        public decimal Rate { get; set; }
        public string Type { get; set; } // e.g., SalesTax, VAT
    }
}