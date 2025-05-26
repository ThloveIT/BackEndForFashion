using AutoMapper;
using BackEndForFashion.Application.Interfaces;
using BackEndForFashion.Application.ViewModels;
using BackEndForFashion.Domain.Entities;
using BackEndForFashion.Domain.Interfaces;

namespace BackEndForFashion.Application.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;

        public PaymentService(IPaymentRepository paymentRepository, IOrderRepository orderRepository, IMapper mapper)
        {
            _paymentRepository = paymentRepository;
            _orderRepository = orderRepository;
            _mapper = mapper;
        }
        public async Task<PaymentVM> ProcessPaymentAsync(PaymentVM model, Guid UserId)
        {
            //Kiem tra don hang
            var order = await _orderRepository.GetByIdAsync(model.OrderId);
            if (order == null)
            {
                throw new Exception("Đơn hàng không tồn tại hoặc bạn không có quyền truy cập.");
            }
            //Kiem tra trang thai
            if (order.Status != Domain.Entities.Status.Pending)
            {
                throw new Exception("Chỉ có thể thanh toán đơn hàng ở trạng thái 'Chờ xác nhận'.");
            }
            //Kiem tra so tien
            if(order.TotalAmount != model.Amount)
            {
                throw new Exception("Số tiền thanh toán không khớp với đơn hàng.");
            }
            //Kiem tra xem don hang da thanh toan chua
            if(order.Payment != null && order.Payment.Status == PaymentStatus.Completed)
            {
                throw new Exception("Đơn hàng đã được thanh toán.");
            }
            //2. Tao ban ghi thanh toan
            var payment = new Payment
            {
                Id = Guid.NewGuid(),
                OrderId = model.OrderId,
                Amount = model.Amount,
                PaymentMethod = model.PaymentMethod,
                CreatedAt = DateTime.UtcNow,
            };
            if(payment.PaymentMethod == PaymentMethod.COD)
            {
                payment.Status = PaymentStatus.Pending;
                order.Status = Status.Confirmed;
            }
            else
            {
                payment.Status = PaymentStatus.Pending;
                try
                {
                    //Goi cong thanh toan
                    payment.Status = PaymentStatus.Completed;
                    order.Status = Status.Confirmed;
                }
                catch(Exception ex) 
                {
                    payment.Status = PaymentStatus.Failed;
                    throw new Exception("Thanh toán thất bại. Vui lòng thử lại.");
                }
            }

            //Luu du lieu 
        
            await _paymentRepository.AddAsync(payment);
            await _orderRepository.UpdateAsync(order);

            return _mapper.Map<PaymentVM>(payment);
        }
    }
}
