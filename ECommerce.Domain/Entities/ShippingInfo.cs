namespace ECommerce.Domain.Entities
{
    public class ShippingInfo
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public Order Order { get; set; }
        public int AddressId { get; set; }
        public Address Address { get; set; }
        public int ShippingMethodId { get; set; }
        public ShippingMethod ShippingMethod { get; set; }
        public string TrackingNumber { get; set; }
        public string Status { get; set; } // e.g., Processing, Shipped, Delivered
    }
}