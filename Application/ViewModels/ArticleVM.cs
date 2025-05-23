namespace BackEndForFashion.Application.ViewModels
{
    public class ArticleVM
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string ThumbnailUrl { get; set; }
        public Guid ArticleCategoryId { get; set; }
        public string ArticleCategoryName { get; set; }
    }
}
