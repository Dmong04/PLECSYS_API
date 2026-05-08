using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APPLICATION.Use_cases.Currencies_case
{
    public class CurrencyRequest
    {
        public required string Currency_ISO { get; set; }

        public required string Currency_code { get; set; }

        public required string Currency_name { get; set; }
    }
}
