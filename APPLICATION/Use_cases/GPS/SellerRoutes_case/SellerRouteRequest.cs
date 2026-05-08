using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APPLICATION.Use_cases.GPS.SellerRoutes_case
{
    public class SellerRouteRequest
    {
        public required string Seller_id { get; set; }

        public required DateTime Timestamp { get; set; }

        public required string Start_location_name { get; set; }

        public required LocationSellerRequest Start_location { get; set; }

        public required string End_location_name { get; set; }

        public required LocationSellerRequest End_location { get; set; }
    }
}
