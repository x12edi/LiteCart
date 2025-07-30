namespace ECommerce.Application.Dtos
{
    public class ReturnRequestDto
    {
        public int Id { get; set; }
        public int OrderItemId { get; set; }
        public string Reason { get; set; }
        public string Status { get; set; }
        public string ResolutionType { get; set; }
    }

    public class CreateReturnRequestDto
    {
        public int OrderItemId { get; set; }
        public string Reason { get; set; }
        public string ResolutionType { get; set; }
    }

    public class UpdateReturnRequestDto
    {
        public string Status { get; set; }
        public string ResolutionType { get; set; }
    }
}