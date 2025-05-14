namespace BackEndForFashion.Entities
{
    public class About
    {
        public Guid Id { get; set; }
        public string Content { get; set; } 
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
