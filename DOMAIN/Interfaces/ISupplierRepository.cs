using DOMAIN.Entities;

namespace DOMAIN.Interfaces
{
    public interface ISupplierRepository
    {
        Task<List<Supplier>> GetSuppliersByCompanyId(int companyId);

        Task<Supplier> GetSupplierById(int supplierId);
    }
}
