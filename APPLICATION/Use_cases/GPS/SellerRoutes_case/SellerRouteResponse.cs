using MongoDB.Bson;
using MongoDB.Driver.GeoJsonObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APPLICATION.Use_cases.GPS.SellerRoutes_case
{
    public class SellerRouteResponse
    {
        public ObjectId? Id { get; set; }

        public string? Seller_id { get; set; }

        public DateTime? Timestamp { get; set; }

        public string? Start_location_name { get; set; }

        public GeoJsonPoint<GeoJson2DGeographicCoordinates>? Start_location_points { get; set; }

        public string? End_location_name { get; set; }

        public GeoJsonPoint<GeoJson2DGeographicCoordinates>? End_location_points { get; set; }
    }
}
