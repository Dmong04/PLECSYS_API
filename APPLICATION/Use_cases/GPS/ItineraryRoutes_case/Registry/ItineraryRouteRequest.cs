using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APPLICATION.Use_cases.GPS.ItineraryRoutes_case.Registry
{
    public class ItineraryRouteRequest
    {
        public string? Reference_id { get; set; }

        public required string Owner_email { get; set; }

        public required string Seller_id { get; set; }
        public DateTime Start_date { get; set; }
        public DateTime End_date { get; set; }
        public List<TargetPointItineraryRequest> Target_points { get; set; } = [];
    }
}
