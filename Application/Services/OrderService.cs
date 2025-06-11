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

        public OrderService(IOrderRepository orderRepository, IOrderDetailRepository orderDetailRepository, IMapper mapper, IProductRepository productRepository, MyDbContext context)
        {
            _context = context;
            _orderRepository = orderRepository;
            _orderDetailRepository = orderDetailRepository;
            _mapper = mapper;
            _productRepository = productRepository;
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
                //Tao don hang moi
                var order = new Order
                {
                    Id = Guid.NewGuid(),
                    UserId = UserId,
                    OrderCode = $"MDH-{DateTime.UtcNow.Ticks}",
                    TotalAmount = model.TotalAmount,
                    ShippingAddress = model.ShippingAddress,
                    Status = Enum.Parse<Status>(model.Status),
                    CreatedAt = DateTime.UtcNow,
                };
                //Tao chi tiet don hang
                var orderDetails = new List<OrderDetail>();
                foreach (var detail in model.OrderDetails)
                {
                    var product = await _productRepository.GetByIdAsync(detail.ProductId);
                    if (product == null || product.Stock < detail.Quantity)
                    {
                        throw new Exception($"Sản phẩm {detail.ProductName} đã hết hàng");
                    }

                    product.Stock -= detail.Quantity;
                    await _productRepository.UpdateAsync(product);

                    orderDetails.Add(new OrderDetail
                    {
                        Id = Guid.NewGuid(),
                        OrderId = order.Id,
                        ProductId = detail.ProductId,
                        Quantity = detail.Quantity,
                        UnitPrice = product.Price,
                    });
                }

                order.OrderDetails = orderDetails;
                await _orderRepository.AddAsync(order);

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

        public async Task<OrderVM> UpdateAsync(Guid id, OrderVM model)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var existingOrder = await _orderRepository.GetByIdAsync(id);
                if (existingOrder == null)
                {
                    throw new Exception("Đơn hàng không tồn tại.");
                }

                existingOrder.OrderCode = model.OrderCode ?? existingOrder.OrderCode;
                existingOrder.TotalAmount = model.TotalAmount;
                existingOrder.ShippingAddress = model.ShippingAddress ?? existingOrder.ShippingAddress;
                existingOrder.Status = string.IsNullOrEmpty(model.Status)
                    ? existingOrder.Status
                    : Enum.Parse<Status>(model.Status);
                existingOrder.UpdatedAt = DateTime.UtcNow;

                if (model.OrderDetails != null && model.OrderDetails.Any())
                {
                    var existingDetails = existingOrder.OrderDetails.ToList();
                    foreach (var detail in existingDetails)
                    {
                        await _orderDetailRepository.DeleteAsync(detail.Id);
                    }

                    var newDetails = new List<OrderDetail>();
                    foreach (var detail in model.OrderDetails)
                    {
                        var product = await _productRepository.GetByIdAsync(detail.ProductId);
                        if (product == null)
                        {
                            throw new Exception($"Sản phẩm với ID {detail.ProductId} không tồn tại.");
                        }

                        newDetails.Add(new OrderDetail
                        {
                            Id = Guid.NewGuid(),
                            OrderId = id,
                            ProductId = detail.ProductId,
                            Quantity = detail.Quantity,
                            UnitPrice = product.Price,
                        });
                    }
                    existingOrder.OrderDetails = newDetails;
                }

                await _orderRepository.UpdateAsync(existingOrder);
                await transaction.CommitAsync();
                return _mapper.Map<OrderVM>(existingOrder);
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}
