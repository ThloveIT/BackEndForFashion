using BackEndForFashion.Domain.Entities;

namespace BackEndForFashion.Application.ViewModels
{

    public class PaymentVM
    {
        public Guid Id { get; set; }
        public Guid OrderId { get; set; }
        public decimal Amount { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public PaymentStatus Status { get; set; }
    }
}
