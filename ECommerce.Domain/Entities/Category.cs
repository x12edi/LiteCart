namespace ECommerce.Domain.Entities
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? ParentId { get; set; } // Nullable for top-level categories
        public Category Parent { get; set; } // Navigation property
        public string? ParentCategory{ get; set; }
        public ICollection<Category> SubCategories { get; set; } = new List<Category>();
    }
}