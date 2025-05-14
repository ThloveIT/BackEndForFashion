namespace BackEndForFashion.Entities
{
    public class ArticleCategory
    {
        public Guid Id { get; set; }
        public string ArticleCategoryName { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }  

        //quan he
        public ICollection<Article> Articles { get; set; }
    }
}
