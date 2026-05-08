using DOMAIN.Entities;
using DOMAIN.Interfaces;
using INFRASTRUCTURE.Context;
using Microsoft.EntityFrameworkCore;

namespace INFRASTRUCTURE.Repositories
{
    public class SupplierRepository(AppDBContext _ctx) : ISupplierRepository
    {
        public async Task<Supplier> GetSupplierById(int supplierId)
        {
            return await _ctx.Suppliers.FirstAsync(s => s.Supplier_id == supplierId);
        }

        public async Task<List<Supplier>> GetSuppliersByCompanyId(int companyId)
        {
            return await _ctx.Suppliers.Include(s => s.Company).ToListAsync();
        }
    }
}
