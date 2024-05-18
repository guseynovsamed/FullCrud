using System;
using Microsoft.EntityFrameworkCore;
using OneToMany.Data;
using OneToMany.Models;

namespace OneToMany.Services.Interface
{
    public class ProductService : IProductService
    {
        private readonly AppDbContext _context;

        public ProductService(AppDbContext context)
        {
            _context = context;
        }


        public async Task<List<Product>> GetAllAsync()
        {
            return await _context.Products.Include(m=>m.ProductImages).ToListAsync();
        }

        public async Task<Product> GetByIdAsync(int? id)
        {
            return await _context.Products.Where(m => !m.SoftDeleted)
                                                     .Include(m => m.Category)
                                                     .Include(m => m.ProductImages)
                                                     .FirstOrDefaultAsync(m => m.Id == id);
        }
    }
}

