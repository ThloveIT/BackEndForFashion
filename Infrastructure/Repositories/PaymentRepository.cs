using BackEndForFashion.Domain.Entities;
using BackEndForFashion.Domain.Interfaces;
using BackEndForFashion.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BackEndForFashion.Infrastructure.Repositories
{
    public class PaymentRepository : Repository<Payment>, IPaymentRepository
    {
        public PaymentRepository(MyDbContext context) : base(context)
        {
        }

        public async Task<Payment> GetByOrderIdAsync(Guid OrderId)
        {
            return await _context.Payments
                 .FirstOrDefaultAsync(p => p.OrderId == OrderId);
        }
    }
}
