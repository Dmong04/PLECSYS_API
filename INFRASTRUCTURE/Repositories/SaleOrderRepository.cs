using DOMAIN.Entities;
using DOMAIN.Interfaces;
using INFRASTRUCTURE.Context;
using Microsoft.EntityFrameworkCore;

namespace INFRASTRUCTURE.Repositories
{
    public class SaleOrderRepository(AppDBContext _ctx) : ISaleOrderRepository
    {
        public async Task<SaleOrder> CreateSaleOrder(SaleOrder order)
        {
            var created = await _ctx.SaleOrders.AddAsync(order);
            await _ctx.SaveChangesAsync();
            var success = created.Entity;
            return success;
        }

        public async Task<SaleOrder> GetSaleOrderById(int order_id)
        {
            return await _ctx.SaleOrders.FirstAsync(o => o.Order_id == order_id);
        }
    }
}
