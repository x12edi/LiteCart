using System.Collections.Generic;

namespace ECommerce.Web.Models
{
    public class CartViewModel
    {
        public int CartId { get; set; }
        public List<CartItemViewModel> Items { get; set; } = new List<CartItemViewModel>();
        public decimal TotalPrice { get; set; }
    }

    public class CartItemViewModel
    {
        public int ProductVariantId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string Size { get; set; }
        public string Color { get; set; }
        public decimal PriceAtTime { get; set; }
        public int Quantity { get; set; }
        public decimal Subtotal => PriceAtTime * Quantity;
        public string ImageBase64 { get; set; }
        public string DisplayName => $"{(Size != null ? Size : "")}{(Size != null && Color != null ? " - " : "")}{(Color != null ? Color : "")}".Trim();
    }
}