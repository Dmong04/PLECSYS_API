using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APPLICATION.Use_cases.Invoices_case
{
    public class InvoiceRequest
    {
        public int Consecutive { get; set; }

        public decimal Total_voucher { get; set; }

        public required string User_creator_id { get; set; }

        public required int Sell_company_id { get; set; }

        public required int Charged_company_id { get; set; }

        public required int Currency_id { get; set; }
    }
}
