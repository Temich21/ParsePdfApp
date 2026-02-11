using ParsePdfApp.Models;

namespace ParsePdfApp.Services
{
    public class CategoryService
    {
        private readonly List<Category> _category;

        public CategoryService()
        {
            _category = new List<Category>
            {
            new Category { Id = Guid.NewGuid(), Name = "Bakery and Confectionery", Expenses = 2000, Color = "#FDEBD0" }, // Warm Beige
            new Category { Id = Guid.NewGuid(), Name = "Fruits and Vegetables", Expenses = 1800, Color = "#D5F5E3" }, // Soft Green
            new Category { Id = Guid.NewGuid(), Name = "Meat and Fish", Expenses = 2500, Color = "#F9E79F" }, // Pale Gold
            new Category { Id = Guid.NewGuid(), Name = "Dairy and Refrigerated", Expenses = 2300, Color = "#D6EAF8" }, // Light Blue
            new Category { Id = Guid.NewGuid(), Name = "Frozen", Expenses = 1700, Color = "#AED6F1" }, // Pale Ice Blue
            new Category { Id = Guid.NewGuid(), Name = "Sausages and Delicacies", Expenses = 1900, Color = "#F5CBA7" }, // Light Salmon
            new Category { Id = Guid.NewGuid(), Name = "Non-Perishables", Expenses = 2100, Color = "#F8E1BA" }, // Pale Brown
            new Category { Id = Guid.NewGuid(), Name = "Beverages", Expenses = 2200, Color = "#FFE5E0" }, // Soft Peach
            new Category { Id = Guid.NewGuid(), Name = "Special Nutrition", Expenses = 1600, Color = "#D7BDE2" }, // Soft Lavender
            new Category { Id = Guid.NewGuid(), Name = "Baby Care", Expenses = 1400, Color = "#FDEDEC" } // Gentle Pink
            };
        }

        public List<Category> getCategoryList() { return _category; }
    }
}
