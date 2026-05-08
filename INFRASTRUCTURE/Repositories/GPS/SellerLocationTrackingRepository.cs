using DOMAIN.Entities.GPS;
using DOMAIN.Interfaces.GPS;
using INFRASTRUCTURE.Context.Mongo;
using MongoDB.Driver;

namespace INFRASTRUCTURE.Repositories.GPS
{
    public class SellerLocationTrackingRepository(MongoCollections _collections)
        : ISellerLocationTrackingRepository
    {
        public async Task SaveAsync(SellerLocationTracking location)
        {
            await _collections._seller_location_tracking.InsertOneAsync(location);
        }

        public async Task<List<SellerLocationTracking>> GetBySellerAndDateRangeAsync(string seller_id, DateTime from, DateTime to)
        {

            var filter = Builders<SellerLocationTracking>.Filter.And(
                Builders<SellerLocationTracking>.Filter.Eq(x => x.SellerId, seller_id),
                Builders<SellerLocationTracking>.Filter.Gte(x => x.Timestamp, from),
                Builders<SellerLocationTracking>.Filter.Lte(x => x.Timestamp, to));
            var sort = Builders<SellerLocationTracking>.Sort.Ascending(x => x.Timestamp);
            return await _collections._seller_location_tracking.Find(filter).Sort(sort).ToListAsync();
        }
    }
}
