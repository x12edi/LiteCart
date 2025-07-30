namespace ECommerce.Application.Dtos
{
    public class WarehouseDto
    {
        public int Id { get; set; }
        public string Location { get; set; }
        public string Manager { get; set; }
        public int Capacity { get; set; }
    }

    public class CreateWarehouseDto
    {
        public string Location { get; set; }
        public string Manager { get; set; }
        public int Capacity { get; set; }
    }

    public class UpdateWarehouseDto
    {
        public string Location { get; set; }
        public string Manager { get; set; }
        public int Capacity { get; set; }
    }
}