using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DOMAIN.Entities
{
    public class Currency
    {
        public int Currency_id { get; set; }

        public required string Currency_ISO { get; set; }

        public required string Currency_code { get; set; }

        public required string Currency_name { get; set; }

        [JsonIgnore]
        public ICollection<PaymentRecord>? Payment_records { get; set; }

        [JsonIgnore]
        public ICollection<Invoice>? Invoices { get; set; }
    }
}
