using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APPLICATION.Use_cases.Products_case
{
    public class ProductResponse
    {
        public int Product_id { get; set; }

        public string? Product_name { get; set; }

        public string? Product_detail { get; set; }

        public decimal Unit_price { get; set; }
    }
}
