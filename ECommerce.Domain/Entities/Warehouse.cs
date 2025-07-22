namespace ECommerce.Domain.Entities
{
    public class Warehouse
    {
        public int Id { get; set; }
        public string Location { get; set; }
        public string Manager { get; set; }
        public int Capacity { get; set; }
    }
}