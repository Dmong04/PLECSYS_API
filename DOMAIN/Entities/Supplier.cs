using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace DOMAIN.Entities
{
    public class Supplier
    {
        public int Supplier_id { get; set; }

        public required string Supplier_name { get; set; }

        [ForeignKey("company_id")]
        [Column("company_id")]
        public int Company_id { get; set; }

        [JsonIgnore]
        public Company? Company { get; set; }

        [JsonIgnore]
        public ICollection<SaleOrder>? Orders { get; set; }
    }
}
