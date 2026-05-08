using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APPLICATION.Use_cases.Currencies_case
{
    public class CurrencyResponse
    {
        public int Currency_id { get; set; }

        public string? Currency_ISO { get; set; }

        public string? Currency_code { get; set; }

        public string? Currency_name { get; set; }
    }
}
