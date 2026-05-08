using DOMAIN.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DOMAIN.Interfaces
{
    public interface ISaleOrderDetailsRepository
    {
        Task<List<SaleOrderDetails>> GetAllSaleOrderDetails();

        Task<bool> CreateSaleOrderDetails(SaleOrderDetails details);
    }
}
