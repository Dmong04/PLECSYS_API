using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DOMAIN.Entities.GPS
{
    public class SellerTrackingConfig
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

        [BsonElement("seller_id")]
        public string SellerId { get; set; } = string.Empty;

        [BsonElement("interval_minutes")]
        public int IntervalMinutes { get; set; } = 30;

        [BsonElement("updated_at")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
