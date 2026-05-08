using DOMAIN.Entities.GPS;

namespace DOMAIN.Interfaces.GPS
{
    public interface ISellerLocationTrackingRepository
    {
        Task SaveAsync(SellerLocationTracking location);

        Task<List<SellerLocationTracking>> GetBySellerAndDateRangeAsync(string seller_id, DateTime from, DateTime to);
    }
}
