using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace DOMAIN.Entities
{
    public class SaleOrder
    {
        public int Order_id { get; set; }

        [ForeignKey("supplier_id")]
        [Column("supplier_id")]
        public int Supplier_id { get; set; }

        [ForeignKey("user_email")]
        [Column("user_email")]
        public required string User_email { get; set; }

        [ForeignKey("company_id")]
        [Column("company_id")]
        public int Company_id { get; set; }

        public DateTime Order_date { get; set; }

        [JsonIgnore]
        public Supplier? Supplier { get; set; }

        [JsonIgnore]
        public User? User { get; set; }

        [JsonIgnore]
        public Company? Company { get; set; }

        [JsonIgnore]
        public ICollection<SaleOrderDetails>? Details { get; set; }
    }
}
