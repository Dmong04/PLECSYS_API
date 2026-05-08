using DOMAIN.Entities.GPS;
using DOMAIN.Interfaces.GPS;
using INFRASTRUCTURE.Context.Mongo;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INFRASTRUCTURE.Repositories.GPS
{
    public class SellerRouteRepository(MongoCollections _ctx) : ISellerRouteRepository
    {
        public async Task<List<SellerRoute>> GetSellerRoutesBySellerId(string seller_id)
        {
            return await _ctx._seller_routes.Find(s => s.Seller_id == seller_id).ToListAsync();
        }

        public async Task<SellerRoute> RegisterSellerRoute(SellerRoute seller_route)
        {
            await _ctx._seller_routes.InsertOneAsync(seller_route);
            return seller_route;
        }
    }
}
