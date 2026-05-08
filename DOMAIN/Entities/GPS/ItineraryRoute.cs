using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace DOMAIN.Entities.GPS
{
    public class ItineraryRoute
    {
        [Key]
        [BsonId]
        public ObjectId Id { get; set; }

        [BsonElement("reference_id")]
        public string? Reference_id { get; set; }         

        [BsonElement("owner_email")]
        public required string Owner_email { get; set; }   

        [BsonElement("seller_id")]
        public required string Seller_id { get; set; }     

        [BsonElement("start_date")]
        public DateTime Start_date { get; set; }           

        [BsonElement("end_date")]
        public DateTime End_date { get; set; }             

        [BsonElement("target_points")]
        public List<TargetPoint> Target_points { get; set; } = [];
    }
}
