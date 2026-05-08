using DOMAIN.Entities.GPS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DOMAIN.Interfaces.GPS
{
    public interface ISellerRouteRepository
    {
        Task<SellerRoute> RegisterSellerRoute(SellerRoute seller_route);

        Task<List<SellerRoute>> GetSellerRoutesBySellerId(string seller_id);
    }
}
