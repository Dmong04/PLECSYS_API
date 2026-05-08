using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver.GeoJsonObjectModel;

namespace DOMAIN.Entities.GPS
{
    public class TargetPoint
    {
        [BsonElement("reference_name")]
        public string? Reference_name { get; set; }

        [BsonElement("client_id")]
        public string? Client_id { get; set; }

        [BsonElement("location")]
        public GeoJsonPoint<GeoJson2DGeographicCoordinates>? Location { get; set; }

        [BsonElement("visited")]
        public bool Visited { get; set; }

        [BsonElement("visiting_time")]
        public DateTime? Visiting_time { get; set; }
    }
}
