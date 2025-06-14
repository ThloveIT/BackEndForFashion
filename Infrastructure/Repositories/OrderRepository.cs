﻿using BackEndForFashion.Domain.Entities;
using BackEndForFashion.Domain.Interfaces;
using BackEndForFashion.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BackEndForFashion.Infrastructure.Repositories
{
    public class OrderRepository : Repository<Order>, IOrderRepository
    {
        public OrderRepository(MyDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Order>> GetAllOrderAsync()
        {
            return await _context.Orders
                .Include(o => o.OrderDetails)
                .ThenInclude(o => o.Product)
                .Include(o => o.Payment)
                .ToListAsync();
        }

        public async Task<Order> GetByUserIdAndOrderIdAsync(Guid UserId, Guid OrderId)
        {
            return await _context.Orders.FirstOrDefaultAsync(o => o.Id == OrderId && o.UserId == UserId);
        }

        public async Task<IEnumerable<Order>> GetByUserIdAsync(Guid UserId)
        {
            return await _context.Orders
                .Where(o=>o.UserId == UserId)
                .Include(o=>o.OrderDetails)
                .ThenInclude(o=>o.Product)
                .Include(o=>o.Payment)
                .ToListAsync();
        }
    }
}
