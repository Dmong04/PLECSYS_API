using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DOMAIN.Entities
{
    public class InvoiceHistory
    {
        public int Invoice_history_id { get; set; }

        [ForeignKey("invoice_id")]
        [Column("invoice_id")]
        public required int Invoice_id { get; set; }

        public DateTime Record_date { get; set; }

        public required string Action { get; set; }

        public required string Description { get; set; }

        [ForeignKey("user_id")]
        [Column("user_id")]
        public string? User_id { get; set; }

        public string? Previous_status { get; set; }

        public string? New_status { get; set; }

        public decimal? Paid_amount { get; set; }

        public decimal? Pending_balance { get; set; }

        [ForeignKey("payment_record_id")]
        [Column("payment_record_id")]
        public int? Payment_record_id { get; set; }

        [ForeignKey("claim_id")]
        [Column("claim_id")]
        public int? Claim_id { get; set; }

        [JsonIgnore]
        public User? User { get; set; }

        [JsonIgnore]
        public Invoice? Invoice { get; set; }

        [JsonIgnore]
        public PaymentRecord? PaymentRecord { get; set; }

        [JsonIgnore]
        public Claim? Claim { get; set; }
    }
}
