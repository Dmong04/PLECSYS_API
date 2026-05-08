using DOMAIN.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DOMAIN.Interfaces
{
    public interface ISaleOrderRepository
    {
        Task<SaleOrder> CreateSaleOrder(SaleOrder order);

        Task<SaleOrder> GetSaleOrderById(int order_id);
    }
}
