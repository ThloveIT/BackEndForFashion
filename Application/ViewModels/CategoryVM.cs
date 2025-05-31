namespace BackEndForFashion.Application.ViewModels
{
    public class CategoryVM
    {
        public Guid Id { get; set; }
        public string CategoryName { get; set; }
        public string Description { get; set; }
        public Guid? ParentId { get; set; }
        public string? ParentName { get; set; }
    }
}
