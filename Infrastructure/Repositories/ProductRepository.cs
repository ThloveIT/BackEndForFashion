﻿using BackEndForFashion.Application.ViewModels;
using BackEndForFashion.Domain.Entities;
using BackEndForFashion.Domain.Interfaces;
using BackEndForFashion.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BackEndForFashion.Infrastructure.Repositories
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository(MyDbContext context) : base(context) { }
        public async Task<IEnumerable<Product>> GetByCategoryAsync(Guid CategoryId)
        {
            return await _context.Products
                .Where(p => p.CategoryId == CategoryId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetFeaturedAsync()
        {
            return await _context.Products
                .Where(p=>p.IsFeatured)
                .Include(p=>p.Category)
                .Include(p=>p.ProductImages)
                .ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetNewAsync()
        {
            var now = DateTime.UtcNow;
            var recentDays = 7;

            //Lay theo thuoc tinh IsNew hoac san pham update trong vong 7 ngay gan nhat
            return await _context.Products
                .Where(p=>p.IsNew || p.CreatedAt >= now.AddDays(-recentDays))
                .Include(p=>p.Category)
                .Include(p=>p.ProductImages)
                .OrderByDescending(p=>p.CreatedAt)
                .Take(10)  // Lay 10 san pham
                .ToListAsync ();
        }

        public async Task<IEnumerable<Product>> GetPopularAsync()
        {
            return await _context.Products
                .Where(p => p.IsPopular)
                .Include(p => p.Category)
                .Include(p => p.ProductImages)
                .Take(10)
                .ToListAsync();
        }

        public async Task<IEnumerable<Product>> SearchAsync(string keyword)
        {
            return await _context.Products
                .Where(p=>p.ProductName.Contains(keyword) || p.Description.Contains(keyword))
                .Include(p=>p.ProductImages)
                .Include(p=>p.Category)
                .ToListAsync();
        }

        public override async Task<Product> GetByIdAsync(Guid Id)
        {
            return await _context.Products
                .Include(p=>p.ProductImages)
                .Include(p=>p.Category)
                .FirstOrDefaultAsync(p => p.Id == Id);
        }
    }
}
