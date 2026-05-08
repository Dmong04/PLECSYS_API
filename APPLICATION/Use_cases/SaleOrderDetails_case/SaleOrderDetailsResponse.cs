using APPLICATION.Use_cases.Products_case;
using APPLICATION.Use_cases.SaleOrders_case;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APPLICATION.Use_cases.SaleOrderDetails_case
{
    public class SaleOrderDetailsResponse
    {
        public int Detail_id { get; set; }

        public SaleOrdersResponse? Order { get; set; }

        public ProductResponse? Product { get; set; }

        public int Quantity { get; set; }

        public decimal Unit_price { get; set; }

        public decimal Subtotal { get; set; }
    }
}
