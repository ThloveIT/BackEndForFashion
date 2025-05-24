using BackEndForFashion.Application.ViewModels;

namespace BackEndForFashion.Application.Interfaces
{
    public interface IPaymentService
    {
        //Xu ly thanh toan cho don hang
        Task<PaymentVM> ProcessPaymentAsync(PaymentVM model, Guid UserId);
    }
}
