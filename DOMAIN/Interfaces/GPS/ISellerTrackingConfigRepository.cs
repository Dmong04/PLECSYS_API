using DOMAIN.Entities.GPS;

namespace DOMAIN.Interfaces.GPS
{
    public interface ISellerTrackingConfigRepository
    {
        Task<SellerTrackingConfig?> GetBySellerIdAsync(string sellerId);
        Task UpsertAsync(SellerTrackingConfig config);
    }
}
