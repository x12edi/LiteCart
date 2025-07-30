namespace ECommerce.Application.Dtos
{
    public class TaxRuleDto
    {
        public int Id { get; set; }
        public string Region { get; set; }
        public decimal Rate { get; set; }
        public string Type { get; set; }
    }

    public class CreateTaxRuleDto
    {
        public string Region { get; set; }
        public decimal Rate { get; set; }
        public string Type { get; set; }
    }

    public class UpdateTaxRuleDto
    {
        public string Region { get; set; }
        public decimal Rate { get; set; }
        public string Type { get; set; }
    }
}