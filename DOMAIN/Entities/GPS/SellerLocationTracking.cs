using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver.GeoJsonObjectModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DOMAIN.Entities.GPS
{
    public class SellerLocationTracking
    {
        [Key]
        [BsonId]
        public ObjectId Id { get; set; }

        [BsonElement("seller_id")]
        public required string SellerId { get; set; }

        [BsonElement("timestamp")]
        public DateTime Timestamp { get; set; }

        [BsonElement("location")]
        public required GeoJsonPoint<GeoJson2DGeographicCoordinates> Location { get; set; }

    }
}
