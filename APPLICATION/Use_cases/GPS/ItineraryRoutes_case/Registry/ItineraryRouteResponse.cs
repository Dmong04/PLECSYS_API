using DOMAIN.Entities.GPS;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APPLICATION.Use_cases.GPS.ItineraryRoutes_case.Registry
{
    public class ItineraryRouteResponse
    {
        public ObjectId? Id { get; set; }
        public string? Reference_id { get; set; }
        public string? Owner_email { get; set; }
        public string? Seller_id { get; set; }

        public DateTime Start_date { get; set; }
        public DateTime End_date { get; set; }

        public List<TargetPoint>? Target_points { get; set; } = [];
    }
}
