﻿namespace ECommerce.Domain.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; } // e.g., Pending, Paid, Shipped
        public DateTime CreatedAt { get; set; }
        public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();
    }
}