namespace BackEndForFashion.Domain.Entities
{
    public class Article
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        //duong dan toi anh dai dien
        public string ThumbnailUrl { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsActive { get; set; }

        //quan he
        public Guid? ArticleCategoryId { get; set; }
        public ArticleCategory ArticleCategory { get; set; }

    }
}
