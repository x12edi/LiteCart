namespace ECommerce.Application.Dtos
{
    public class ShippingInfoDto
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int AddressId { get; set; }
        public int ShippingMethodId { get; set; }
        public string Status { get; set; }
        public string TrackingNumber { get; set; }
    }

    public class CreateShippingInfoDto
    {
        public int OrderId { get; set; }
        public int AddressId { get; set; }
        public int ShippingMethodId { get; set; }
        public string Status { get; set; }
        public string TrackingNumber { get; set; }
    }

    public class UpdateShippingInfoDto
    {
        public string Status { get; set; }
        public string TrackingNumber { get; set; }
    }
}