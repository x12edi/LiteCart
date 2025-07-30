namespace ECommerce.Application.Dtos
{
    public class ShippingMethodDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Cost { get; set; }
        public string EstimatedTime { get; set; }
        public string AvailabilityRegion { get; set; }
    }

    public class CreateShippingMethodDto
    {
        public string Name { get; set; }
        public decimal Cost { get; set; }
        public string EstimatedTime { get; set; }
        public string AvailabilityRegion { get; set; }
    }

    public class UpdateShippingMethodDto
    {
        public string Name { get; set; }
        public decimal Cost { get; set; }
        public string EstimatedTime { get; set; }
        public string AvailabilityRegion { get; set; }
    }
}