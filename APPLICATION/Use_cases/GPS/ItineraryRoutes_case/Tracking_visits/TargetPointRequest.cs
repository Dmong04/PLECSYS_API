using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APPLICATION.Use_cases.GPS.ItineraryRoutes_case.Tracking_visits
{
    public class TargetPointRequest
    {
        public ObjectId Id { get; set; }

        public required string Seller_id { get; set; }

        public DateTime Timestamp { get; set; }

        public required string Reference_name { get; set; }

        public required TargetLocationRequest Location { get; set; }
    }
}
