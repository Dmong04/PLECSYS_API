using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APPLICATION.Use_cases.PaymentRecords_case
{
    public class PaymentRecordRequest
    {
        public required int Source_id { get; set; }

        public required int Currency_id { get; set; }

        public required int Payment_method_id { get; set; }

        public string? Detail_payment_method { get; set; }

        public required decimal Paid_amount { get; set; }

        public required DateTime Payment_date { get; set; }

        public required string Payment_detail { get; set; }

        public required string Third_party_transaction_id { get; set; }
    }
}
