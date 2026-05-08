using MongoDB.Bson;

namespace APPLICATION.Use_cases.GPS.ItineraryRoutes_case.Compare
{
    public class ComparePointResult
    {
        public string? Reference_name { get; set; }

        public string? Client_id { get; set; }

        public bool Visited { get; set; }

        public DateTime? Visiting_time { get; set; }

        public double? Min_distance_meters { get; set; }
    }

    public class CompareItineraryResponse
    {
        public ObjectId? Id { get; set; }

        public string? Seller_id { get; set; }

        public List<ComparePointResult>? Point_results { get; set; } = [];

        public int Total_points { get; set; }

        public int Visited_count { get; set; }

        public double Compliance_percentage { get; set; }
    }
}
