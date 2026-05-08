using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APPLICATION.Use_cases.GPS.ItineraryRoutes_case.Registry
{
    public class GeoJsonLocationRequest
    {
        public string Type { get; set; } = "Point";

        public double[] Coordinates { get; set; } = [];
    }
}
