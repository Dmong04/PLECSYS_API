using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DOMAIN.Entities
{
    public class PaymentMethod
    {
        public int Payment_method_id { get; set; }

        public required string Payment_method_name { get; set; }

        public required int Payment_method_code { get; set; }

        [JsonIgnore]
        public ICollection<PaymentRecord>? Payment_records { get; set; }
    }
}
