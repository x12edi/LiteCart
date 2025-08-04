namespace ECommerce.Application.Dtos
{
    public class CategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? ParentId { get; set; }
        public string? ParentCategory { get; set; }
    }

    public class CreateCategoryDto
    {
        public string Name { get; set; }
        public int? ParentId { get; set; }
    }

    public class UpdateCategoryDto
    {
        public string Name { get; set; }
        public int? ParentId { get; set; }
    }
}