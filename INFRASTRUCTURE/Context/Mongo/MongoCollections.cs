using DOMAIN.Entities.GPS;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INFRASTRUCTURE.Context.Mongo
{
    public class MongoCollections(IMongoDatabase _database)
    {
        public IMongoCollection<ItineraryRoute> _itinerary_routes =
            _database.GetCollection<ItineraryRoute>("Itinerary_routes");

        public IMongoCollection<SellerRoute> _seller_routes =
            _database.GetCollection<SellerRoute>("Seller_routes");

        public IMongoCollection<SellerLocationTracking> _seller_location_tracking =
            _database.GetCollection<SellerLocationTracking>("Seller_location_tracking");

        public IMongoCollection<SellerTrackingConfig> _seller_tracking_configs =
        _database.GetCollection<SellerTrackingConfig>("Seller_tracking_configs");
    }
}
