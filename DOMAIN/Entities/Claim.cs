using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DOMAIN.Entities
{
    public class Claim
    {
        public int Claim_id { get; set; }

        public DateTime Record_date { get; set; }

        [ForeignKey("user_id")]
        [Column("user_id")]
        public required string User_id { get; set; }

        public required string Description { get; set; }

        [ForeignKey("invoice_id")]
        [Column("invoice_id")]
        public required int Invoice_id { get; set; }

        public decimal Claim_amount { get; set; }

        [JsonIgnore]
        public User? User { get; set; }

        [JsonIgnore]
        public Invoice? Invoice { get; set; }

        [JsonIgnore]
        public ICollection<InvoiceHistory>? Invoice_histories { get; set; }
    }
}
