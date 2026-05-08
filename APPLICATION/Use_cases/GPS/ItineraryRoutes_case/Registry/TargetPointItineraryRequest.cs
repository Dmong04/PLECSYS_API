using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APPLICATION.Use_cases.GPS.ItineraryRoutes_case.Registry
{
    public class TargetPointItineraryRequest
    {
        public string? Reference_name { get; set; }
        public string? Client_id { get; set; }

        public GeoJsonLocationRequest Location { get; set; }

        public bool Visited { get; set; }

        public DateTime? Visiting_time { get; set; }
    }
}
