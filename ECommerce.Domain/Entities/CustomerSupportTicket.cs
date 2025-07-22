namespace ECommerce.Domain.Entities
{
    public class CustomerSupportTicket
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public string Subject { get; set; }
        public string Description { get; set; }
        public string Status { get; set; } // e.g., Open, InProgress, Resolved
        public int? AssignedAdminId { get; set; } // Nullable for unassigned
        public User AssignedAdmin { get; set; }
    }
}