namespace ECommerce.Application.Dtos
{
    public class CustomerSupportTicketDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Subject { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public int? AssignedAdminId { get; set; }
    }

    public class CreateCustomerSupportTicketDto
    {
        public int UserId { get; set; }
        public string Subject { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public int? AssignedAdminId { get; set; }
    }

    public class UpdateCustomerSupportTicketDto
    {
        public string Status { get; set; }
        public int? AssignedAdminId { get; set; }
    }
}