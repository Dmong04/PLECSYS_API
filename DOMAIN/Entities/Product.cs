using System.Text.Json.Serialization;

namespace DOMAIN.Entities
{
    public class Product
    {
        public int Product_id { get; set; }

        public required string Product_name { get; set; }

        public required decimal Unit_price { get; set; }

        [JsonIgnore]
        public ICollection<SaleOrderDetails>? Details { get; set; }
    }
}
