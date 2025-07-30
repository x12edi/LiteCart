namespace ECommerce.Application.Dtos
{
    public class StockMovementDto
    {
        public int Id { get; set; }
        public int ProductVariantId { get; set; }
        public int? FromWarehouseId { get; set; }
        public int? ToWarehouseId { get; set; }
        public int Quantity { get; set; }
        public string Reason { get; set; }
        public DateTime Timestamp { get; set; }
    }

    public class CreateStockMovementDto
    {
        public int ProductVariantId { get; set; }
        public int? FromWarehouseId { get; set; }
        public int? ToWarehouseId { get; set; }
        public int Quantity { get; set; }
        public string Reason { get; set; }
    }
}