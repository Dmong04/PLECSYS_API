using MongoDB.Driver.GeoJsonObjectModel;

namespace APPLICATION.Utils.GPS
{
    public class GeoFenceService
    {
        private const double RadiousMeters = 50;

        public bool IsInsideGeoFence(GeoJsonPoint<GeoJson2DGeographicCoordinates> sellerPoint,
            GeoJsonPoint<GeoJson2DGeographicCoordinates> targetPoint)
        {
            if (targetPoint is null)
                return false;
            var seller = sellerPoint.Coordinates;
            var target = targetPoint.Coordinates;

            var distance = HaversineDistance(
                seller.Latitude, seller.Longitude,
                target.Latitude, target.Longitude);

            return distance <= RadiousMeters;
        }

        public double GetDistanceInMeters(GeoJsonPoint<GeoJson2DGeographicCoordinates> point1,
            GeoJsonPoint<GeoJson2DGeographicCoordinates> point2)
        {
            if (point1 is null || point2 is null)
                return double.MaxValue;
            var c1 = point1.Coordinates;
            var c2 = point2.Coordinates;
            return HaversineDistance(c1.Latitude, c1.Longitude, c2.Latitude, c2.Longitude);
        }

        private static double HaversineDistance(double lat1, double lon1, double lat2, double lon2)
        {
            const double earth_radious = 6371000;
            var dlat = ToRadians(lat2 - lat1);
            var dlon = ToRadians(lon2 - lon1);

            var a = Math.Sin(dlat / 2) * Math.Sin(dlat / 2) +
                Math.Cos(ToRadians(lat1)) * Math.Cos(ToRadians(lat2)) *
                Math.Sin(dlon / 2) * Math.Sin(dlon / 2);

            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            return earth_radious * c;
        }

        private static double ToRadians(double angle) => angle * Math.PI / 180;
    }
}
