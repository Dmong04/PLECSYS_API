using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APPLICATION.Use_cases.PaymentMethods_case
{
    public class PaymentMethodResponse
    {
        public int Payment_method_id { get; set; }

        public string? Payment_method_name { get; set; }

        public int? Payment_method_code { get; set; }
    }
}
