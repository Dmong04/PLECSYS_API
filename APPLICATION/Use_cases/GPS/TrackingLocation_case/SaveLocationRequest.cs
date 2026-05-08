using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APPLICATION.Use_cases.GPS.TrackingLocation_case
{
    public class SaveLocationRequest
    {
        public required string SellerId { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
