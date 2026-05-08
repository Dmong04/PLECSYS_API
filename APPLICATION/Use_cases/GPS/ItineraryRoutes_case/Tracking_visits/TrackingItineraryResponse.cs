using DOMAIN.Entities.GPS;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APPLICATION.Use_cases.GPS.ItineraryRoutes_case.Tracking_visits
{
    public class TrackingItineraryResponse
    {
        public ObjectId? Id { get; set; }

        public string? Seller_id { get; set; }

        public List<TargetPoint>? Target_points { get; set; } = [];

        public string? Status { get; set; }

        public string? Message { get; set; }
    }
}
