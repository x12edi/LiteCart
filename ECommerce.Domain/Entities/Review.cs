﻿namespace ECommerce.Domain.Entities
{
    public class Review
    {
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public int Rating { get; set; } // e.g., 1-5
        public string Comment { get; set; }
        public DateTime CreatedAt { get; set; }
    }
        
}