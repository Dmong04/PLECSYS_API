using DOMAIN.Entities.GPS;
using DOMAIN.Interfaces.GPS;
using INFRASTRUCTURE.Context.Mongo;
using MongoDB.Driver;

namespace INFRASTRUCTURE.Repositories.GPS
{
    public class SellerTrackingConfigRepository(MongoCollections _collections)
    : ISellerTrackingConfigRepository
    {
        public async Task<SellerTrackingConfig?> GetBySellerIdAsync(string sellerId)
        {
            return await _collections._seller_tracking_configs
                .Find(x => x.SellerId == sellerId)
                .FirstOrDefaultAsync();
        }

        public async Task UpsertAsync(SellerTrackingConfig config)
        {
            var filter = Builders<SellerTrackingConfig>.Filter
                .Eq(x => x.SellerId, config.SellerId);

            var update = Builders<SellerTrackingConfig>.Update
                .Set(x => x.IntervalMinutes, config.IntervalMinutes)
                .Set(x => x.UpdatedAt, DateTime.UtcNow)
                .SetOnInsert(x => x.SellerId, config.SellerId);

            var options = new UpdateOptions { IsUpsert = true };

            await _collections._seller_tracking_configs
                .UpdateOneAsync(filter, update, options);
        }
    }
}
