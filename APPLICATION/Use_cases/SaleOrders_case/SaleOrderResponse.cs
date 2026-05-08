using APPLICATION.Use_cases.Users_case;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APPLICATION.Use_cases.SaleOrders_case
{
    public class SaleOrdersResponse
    {
        public int Order_id { get; set; }

        public UserResponse? Client { get; set; }

        public DateTime Order_date { get; set; }
    }
}
