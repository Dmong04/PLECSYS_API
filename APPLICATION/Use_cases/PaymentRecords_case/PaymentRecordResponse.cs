using DOMAIN.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace APPLICATION.Use_cases.PaymentRecords_case
{
    public class PaymentRecordResponse
    {
        public int Payment_record_id { get; set; }

        public Invoice? Source { get; set; }

        public Currency? Currency { get; set; }

        public PaymentMethod? Payment_method { get; set; }

        public string? Detail_payment_method { get; set; }

        public decimal? Paid_amount { get; set; }

        public DateTime? Payment_date { get; set; }

        public string? Payment_detail { get; set; }

        public string? Third_party_transaction_id { get; set; }
    }
}
