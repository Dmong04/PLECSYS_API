using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APPLICATION.Use_cases.Invoices_case
{
    public class GetInvoicesRequest
    {
        public required string Email { get; set; }

        public required int CompanyId { get; set; }
    }
}
