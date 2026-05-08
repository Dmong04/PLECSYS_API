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
    public class SaleOrderDetailsRepository(AppDBContext _ctx) : ISaleOrderDetailsRepository
    {
        public async Task<bool> CreateSaleOrderDetails(SaleOrderDetails details)
        {
            await _ctx.SaleOrderDetails.AddAsync(details);
            var rows = await _ctx.SaveChangesAsync();
            return rows > 0;
        }

        public async Task<List<SaleOrderDetails>> GetAllSaleOrderDetails()
        {
            return await _ctx.SaleOrderDetails.Include(d => d.Order).Include(d => d.Product).ToListAsync();
        }
    }
}
