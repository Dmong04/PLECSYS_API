using DOMAIN.Entities.GPS;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DOMAIN.Interfaces.GPS
{
    public interface IItineraryRouteRepository
    {
        Task<ItineraryRoute> GetItineraryRouteById(ObjectId id);
        Task<List<ItineraryRoute>> GetItineraryRoutesBySellerId(string seller_id);
        Task<List<ItineraryRoute>> GetItineraryRoutesByOwnerAsync(string owner_email);
        Task<List<ItineraryRoute>> GetItineraryRoutesByDateRangeAsync(string seller_id, DateTime from, DateTime to);
        Task<ItineraryRoute?> GetItineraryRouteByReferenceIdAsync(string reference_id);
        Task<ItineraryRoute> RegisterItineraryRoute(ItineraryRoute itinerary_route);
        Task<bool> UpdateVisitedPointsAsync(ObjectId itinerary_id, int index, DateTime timestamp);
        Task<bool> UpdatePointClientIdAsync(ObjectId itinerary_id, int index, string client_id);

    }
}
