namespace BackEndForFashion.Application.ViewModels
{
    public class ContactVM
    {
        public Guid Id { get; set; }
        public string ContactName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Message { get; set; }
        public bool IsResolved { get; set; }
    }
}
