namespace BackEndForFashion.Entities
{
    public class Category
    {
        public Guid Id { get; set; }
        public string CategoryName { get; set; }
        public string Description { get; set; }
        
        public bool IsActive { get; set; }

        // quan hệ 

        //mot category co 1 parent
        public Guid? ParentId { get; set; }
        public Category Parent { get; set; }
        // mot category co nhieu san pham
        public ICollection<Product> Products { get; set; }
        //mot category co nhieu subcategory
        public ICollection<Category> SubCategories { get; set; }

    }
}
