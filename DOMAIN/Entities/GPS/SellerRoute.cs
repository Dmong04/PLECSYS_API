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
    public class SellerRoute
    {
        [Key]
        [BsonId]
        public ObjectId Id { get; set; }

        [BsonElement("seller_id")]
        public required string Seller_id { get; set; }

        [BsonElement("timestamp")]
        public DateTime Timestamp { get; set; }

        [BsonElement("start_location_name")]
        public required string Start_location_name { get; set; }

        [BsonElement("start_location_points")]
        public required GeoJsonPoint<GeoJson2DGeographicCoordinates> Start_location_points { get; set; }

        [BsonElement("end_location_name")]
        public required string End_location_name { get; set; }

        [BsonElement("end_location_points")]
        public required GeoJsonPoint<GeoJson2DGeographicCoordinates> End_location_points { get; set; }
    }
}
