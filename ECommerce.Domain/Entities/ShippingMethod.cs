namespace ECommerce.Domain.Entities
{
    public class ShippingMethod
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Cost { get; set; }
        public string EstimatedTime { get; set; } // e.g., 3-5 days
        public string AvailabilityRegion { get; set; }
    }
}