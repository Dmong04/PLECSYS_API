using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APPLICATION.Use_cases.PaymentMethods_case
{
    public class PaymentMethodRequest
    {
        public required string Payment_method_name { get; set; }

        public required int Payment_method_code { get; set; }
    }
}
