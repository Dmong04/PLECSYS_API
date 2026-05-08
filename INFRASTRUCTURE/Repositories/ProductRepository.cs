using DOMAIN.Entities;
using DOMAIN.Interfaces;
using INFRASTRUCTURE.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INFRASTRUCTURE.Repositories
{
    public class ProductRepository(AppDBContext _ctx) : IProductRepository
    {
        public async Task<List<Product>> GetAllProducts()
        {
            return await _ctx.Products.ToListAsync();
        }

        public async Task<Product> GetProductById(int product_id)
        {
            return await _ctx.Products.FindAsync(product_id);
        }

        public async Task<List<Product>> GetProductsByName(string query)
        {
            return await _ctx.Products.Where(p => p.Product_name.ToLower().Contains(query.ToLower()))
                .OrderBy(p => p.Product_name.ToLower().StartsWith(query.ToLower()) ? 0 : 1)
                .ThenBy(p => p.Product_name).Take(3).ToListAsync();
        }
    }
}
