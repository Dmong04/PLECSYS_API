using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DOMAIN.Entities
{
    public class PaymentRecord
    {
        [Key]
        public int Payment_record_id { get; set; }

        [ForeignKey("source_id")]
        [Column("source_id")]
        public int Source_id { get; set; }

        [ForeignKey("currency_id")]
        [Column("currency_id")]
        public required int Currency_id { get; set; }

        [ForeignKey("payment_method_id")]
        [Column("payment_method_id")]
        public required int Payment_method_id { get; set; }

        public string? Detail_payment_method { get; set; }

        public required decimal Paid_amount { get; set; }

        public required DateTime Payment_date { get; set; }

        public required string Payment_detail { get; set; }

        public required string Third_party_transaction_id { get; set; }

        [JsonIgnore]
        public Invoice? Source { get; set; }

        [JsonIgnore]
        public Currency? Currency { get; set; }

        [JsonIgnore]
        public PaymentMethod? Payment_method { get; set; }

        [JsonIgnore]
        public ICollection<InvoiceHistory>? Invoice_histories { get; set; }
    }
}
