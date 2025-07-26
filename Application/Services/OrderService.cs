using AutoMapper;
using BackEndForFashion.Application.Interfaces;
using BackEndForFashion.Application.ViewModels;
using BackEndForFashion.Domain.Entities;
using BackEndForFashion.Domain.Interfaces;
using BackEndForFashion.Infrastructure.Data;

namespace BackEndForFashion.Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly MyDbContext _context;
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderDetailRepository _orderDetailRepository;
        private readonly IMapper _mapper;
        private readonly IProductRepository _productRepository;
        private readonly ICartRepository _cartRepository;
        private readonly ICartItemRepository _cartItemRepository;
        private readonly IPaymentRepository _paymentRepositor;
        private readonly IPaymentService _paymentService;

        public OrderService(IOrderRepository orderRepository, IOrderDetailRepository orderDetailRepository, IMapper mapper, IProductRepository productRepository, MyDbContext context, ICartRepository cartRepository, ICartItemRepository cartItemRepository, IPaymentRepository paymentRepository, IPaymentService paymentService)
        {
            _context = context;
            _orderRepository = orderRepository;
            _orderDetailRepository = orderDetailRepository;
            _mapper = mapper;
            _productRepository = productRepository;
            _cartRepository = cartRepository;
            _cartItemRepository = cartItemRepository;
            _paymentRepositor = paymentRepository;
            _paymentService = paymentService;
        }
        public async Task CancelAsync(Guid UserId, Guid OrderId)
        {
            var order = await _orderRepository.GetByUserIdAndOrderIdAsync(UserId, OrderId);
            if (order == null)
            {
                throw new Exception("Đơn hàng không tồn tại hoặc không thuộc về người dùng");
            }
            if(order.Status != Status.Pending && order.Status != Status.Confirmed)
            {
                throw new Exception("Đơn hàng không thể hủy ở trạng thái hiện tại.");
            }

            order.Status = Status.Cancelled;
            order.UpdatedAt = DateTime.UtcNow;

            await _orderRepository.UpdateAsync(order);
        }

        public async Task<OrderVM> CreateAsync(OrderVM model, Guid UserId)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                //Lay gio hang cua nguoi dung
                var cart = await _cartRepository.GetActiveCartByUserIdAsync(UserId);
                if (cart == null || !cart.CartItems.Any())
                {
                    throw new Exception("Giỏ hàng trống");
                }
                //Tao don hang moi
                var order = new Order
                {
                    Id = Guid.NewGuid(),
                    UserId = UserId,
                    OrderCode = $"MDH-{DateTime.UtcNow.Ticks}",
                    TotalAmount = cart.CartItems.Sum(i => i.Quantity * i.Product.Price),
                    ShippingAddress = model.ShippingAddress,
                    ContactEmail = model.ContactEmail,
                    ContactName = model.ContactName,
                    ContactPhoneNumber = model.ContactPhoneNumber,
                    Status = Status.Pending,
                    CreatedAt = DateTime.UtcNow,
                };
                //Tao chi tiet don hang
                var orderDetails = new List<OrderDetail>();
                foreach (var item in cart.CartItems)
                {
                    var product = await _productRepository.GetByIdAsync(item.ProductId);
                    if (product == null || product.Stock < item.Quantity)
                    {
                        throw new Exception($"Sản phẩm {product?.ProductName} đã hết hàng");
                    }

                    product.Stock -= item.Quantity;
                    await _productRepository.UpdateAsync(product);

                    orderDetails.Add(new OrderDetail
                    {
                        Id = Guid.NewGuid(),
                        OrderId = order.Id,
                        ProductId = item.ProductId,
                        Quantity = item.Quantity,
                        UnitPrice = product.Price,
                    });
                }

                order.OrderDetails = orderDetails;
                await _orderRepository.AddAsync(order);

                var payment = new Payment
                {
                    Id = Guid.NewGuid() ,
                    OrderId = order.Id,
                    Amount = order.TotalAmount,
                    PaymentMethod = model.PaymentMethod,
                    Status = PaymentStatus.Pending,
                    CreatedAt = DateTime.UtcNow,
                };
                await _paymentRepositor.AddAsync(payment);

                //xu ly thanh toan
                if(payment.PaymentMethod == PaymentMethod.COD)
                {
                    order.Status = Status.Confirmed;
                    await _orderRepository.UpdateAsync(order);
                }
                //con else de xu ly thanh toan online
                //Xoa gio hang sau khi dat hang
                foreach (var item in cart.CartItems)
                {
                    await _cartItemRepository.DeleteAsync(item.Id);
                }

                await transaction.CommitAsync();
                return _mapper.Map<OrderVM>(order);
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<IEnumerable<OrderVM>> GetAllOrderAsync()
        {
            var orders = await _orderRepository.GetAllOrderAsync();
            return _mapper.Map<IEnumerable<OrderVM>>(orders);
        }

        public async Task<OrderVM> GetByIdAsync(Guid Id)
        {
            var order = await _orderRepository.GetByIdAsync(Id);
            if (order == null) throw new Exception("Không thấy đơn hàng");
            return _mapper.Map<OrderVM>(order);
        }

        public async Task<IEnumerable< OrderVM>> GetByUserIdAsync(Guid UserId)
        {
            var order = await _orderRepository.GetByUserIdAsync(UserId);
            return _mapper.Map<IEnumerable<OrderVM>>(order);
        }

        public async Task<OrderVM> UpdateAsync(Guid id, UpdateOrderStatus model)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var existingOrder = await _orderRepository.GetByIdAsync(id);
                if (existingOrder == null)
                {
                    throw new Exception("Đơn hàng không tồn tại.");
                }

                // Chỉ cập nhật trạng thái
                if (!string.IsNullOrEmpty(model.Status))
                {
                    existingOrder.Status = Enum.Parse<Status>(model.Status);
                    existingOrder.UpdatedAt = DateTime.UtcNow;
                    await _orderRepository.UpdateAsync(existingOrder);
                }

                await transaction.CommitAsync();
                return _mapper.Map<OrderVM>(existingOrder);
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
        //public async Task<OrderVM> UpdateAsync(Guid id, OrderVM model)
        //{
        //    using var transaction = await _context.Database.BeginTransactionAsync();
        //    try
        //    {
        //        var existingOrder = await _orderRepository.GetByIdAsync(id);
        //        if (existingOrder == null)
        //        {
        //            throw new Exception("Đơn hàng không tồn tại.");
        //        }
        //        existingOrder.OrderCode = model.OrderCode ?? existingOrder.OrderCode;
        //        existingOrder.TotalAmount = model.TotalAmount;
        //        existingOrder.ShippingAddress = model.ShippingAddress ?? existingOrder.ShippingAddress;
        //        existingOrder.Status = string.IsNullOrEmpty(model.Status)
        //            ? existingOrder.Status
        //            : Enum.Parse<Status>(model.Status);
        //        existingOrder.UpdatedAt = DateTime.UtcNow;
        //        if (model.OrderDetails != null && model.OrderDetails.Any())
        //        {
        //            var existingDetails = existingOrder.OrderDetails.ToList();
        //            foreach (var detail in existingDetails)
        //            {
        //                await _orderDetailRepository.DeleteAsync(detail.Id);
        //            }

        //            var newDetails = new List<OrderDetail>();
        //            foreach (var detail in model.OrderDetails)
        //            {
        //                var product = await _productRepository.GetByIdAsync(detail.ProductId);
        //                if (product == null)
        //                {
        //                    throw new Exception($"Sản phẩm với ID {detail.ProductId} không tồn tại.");
        //                }

        //                newDetails.Add(new OrderDetail
        //                {
        //                    Id = Guid.NewGuid(),
        //                    OrderId = id,
        //                    ProductId = detail.ProductId,
        //                    Quantity = detail.Quantity,
        //                    UnitPrice = product.Price,
        //                });
        //            }
        //            existingOrder.OrderDetails = newDetails;
        //        }

        //        await _orderRepository.UpdateAsync(existingOrder);
        //        await transaction.CommitAsync();
        //        return _mapper.Map<OrderVM>(existingOrder);
        //    }
        //    catch
        //    {
        //        await transaction.RollbackAsync();
        //        throw;
        //    }
        //}
    }
}
