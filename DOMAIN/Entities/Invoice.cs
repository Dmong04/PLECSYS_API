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
    public class Invoice
    {
        [Key]
        public int Invoice_id { get; set; }

        public int Consecutive {  get; set; }

        public decimal Total_voucher { get; set; }

        [ForeignKey("user_creator_id")]
        [Column("user_creator_id")]
        public required string User_creator_id { get; set; }

        [ForeignKey("sell_company_id")]
        [Column("sell_company_id")]
        public required int Sell_company_id { get; set; }

        [ForeignKey("charged_company_id")]
        [Column("charged_company_id")]
        public required int Charged_company_id { get; set; }

        [ForeignKey("currency_id")]
        [Column("currency_id")]
        public required int Currency_id { get; set; }

        public string? Status { get; set; }

        public decimal? Pending_balance { get; set; }

        public DateTime Created_at { get; set; }

        [JsonIgnore]
        public Company? Charged_company { get; set; }

        [JsonIgnore]
        public Company? Sell_company { get; set; }

        [JsonIgnore]
        public User? User { get; set; }

        [JsonIgnore]
        public ICollection<Claim>? Claims { get; set; }

        [JsonIgnore]
        public ICollection<InvoiceHistory>? Invoice_histories { get; set; }

        [JsonIgnore]
        public ICollection<PaymentRecord>? Payment_records { get; set; }

        [JsonIgnore]
        public Currency? Currency { get; set; }
    }
}
