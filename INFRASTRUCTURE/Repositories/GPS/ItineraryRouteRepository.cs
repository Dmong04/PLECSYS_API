using DOMAIN.Entities.GPS;
using DOMAIN.Interfaces.GPS;
using INFRASTRUCTURE.Context.Mongo;
using MongoDB.Bson;
using MongoDB.Driver;

namespace INFRASTRUCTURE.Repositories.GPS
{
    public class ItineraryRouteRepository(MongoCollections _ctx) : IItineraryRouteRepository
    {
        public async Task<ItineraryRoute> GetItineraryRouteById(ObjectId id)
        {
            var cursor = await _ctx._itinerary_routes.FindAsync(i => i.Id == id);
            var found = await cursor.FirstOrDefaultAsync();
            return found;
        }


        public async Task<ItineraryRoute> RegisterItineraryRoute(ItineraryRoute itinerary_route)
        {
            await _ctx._itinerary_routes.InsertOneAsync(itinerary_route);
            return itinerary_route;
        }


        public async Task<bool> UpdateVisitedPointsAsync(ObjectId itinerary_id, int index, DateTime timestamp)
        {
            var update = Builders<ItineraryRoute>.Update
                .Set($"target_points.{index}.visited", true)
                .Set($"target_points.{index}.visiting_time", timestamp);

            var updated = await _ctx._itinerary_routes.UpdateOneAsync(Builders<ItineraryRoute>.Filter.Eq(i => i.Id, itinerary_id), update);
            
            if (updated is null) return false;
            return true;
        }


        public async Task<List<ItineraryRoute>> GetItineraryRoutesBySellerId(string seller_id)
        {
            return await _ctx._itinerary_routes.Find(i => i.Seller_id == seller_id).ToListAsync();
        }


        public async Task<List<ItineraryRoute>> GetItineraryRoutesByOwnerAsync(string owner_email)
        {
            return await _ctx._itinerary_routes
                .Find(i => i.Owner_email == owner_email)
                .ToListAsync();
        }


        public async Task<List<ItineraryRoute>> GetItineraryRoutesByDateRangeAsync(
            string seller_id, DateTime from, DateTime to)
        {
            var filter = Builders<ItineraryRoute>.Filter.And(
                Builders<ItineraryRoute>.Filter.Eq(i => i.Seller_id, seller_id),
                Builders<ItineraryRoute>.Filter.Lte(i => i.Start_date, to),   
                Builders<ItineraryRoute>.Filter.Gte(i => i.End_date, from)   
             );
          
            return await _ctx._itinerary_routes
                .Find(filter)
                .ToListAsync();
        }


        public async Task<ItineraryRoute?> GetItineraryRouteByReferenceIdAsync(string reference_id)
        {
            return await _ctx._itinerary_routes
                .Find(i => i.Reference_id == reference_id)
                .FirstOrDefaultAsync();
        }

        //Assign client_id to a specific point by geofence
        public async Task<bool> UpdatePointClientIdAsync(
            ObjectId itinerary_id, int index, string client_id)
        {
            var update = Builders<ItineraryRoute>.Update
                .Set($"target_points.{index}.client_id", client_id);

            var updated = await _ctx._itinerary_routes
                .UpdateOneAsync(
                    Builders<ItineraryRoute>.Filter.Eq(i => i.Id, itinerary_id),
                    update);

            return updated is not null;
        }

    }
}
