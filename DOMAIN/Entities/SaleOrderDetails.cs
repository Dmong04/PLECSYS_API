using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DOMAIN.Entities
{
    public class SaleOrderDetails
    {
        public int Detail_id { get; set; }

        [ForeignKey("order_id")]
        [Column("order_id")]
        public required int Order_id { get; set; }

        [ForeignKey("product_id")]
        [Column("product_id")]
        public required int Product_id { get; set; }

        public required int Quantity { get; set; }

        public decimal Tax_rate { get; set; }

        public decimal Tax { get; set; }

        public decimal Discount { get; set; }

        public decimal Unit_price { get; set; }

        public decimal Subtotal { get; set; }

        public decimal Total { get; set; }

        [JsonIgnore]
        public SaleOrder? Order { get; set; }

        [JsonIgnore]
        public Product? Product { get; set; }
    }
}
