using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APPLICATION.Use_cases.InvoiceHistories_case
{
    public class InvoiceHistoryRequest
    {
        public required int Invoice_id { get; set; }

        public required DateTime Record_date { get; set; }

        public required string Action { get; set; }

        public required string Description { get; set; }
    }
}
