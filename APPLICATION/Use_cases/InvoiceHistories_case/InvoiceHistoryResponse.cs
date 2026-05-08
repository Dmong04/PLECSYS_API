using DOMAIN.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace APPLICATION.Use_cases.InvoiceHistories_case
{
    public class InvoiceHistoryResponse
    {
        public int? Invoice_history_id { get; set; }

        public int? Invoice_id { get; set; }

        public DateTime? Record_date { get; set; }

        public string? Action { get; set; }

        public string? Description { get; set; }

        public string? User_id { get; set; }

        public string? Previous_status { get; set; }

        public string? New_status { get; set; }

        public decimal? Paid_amount { get; set; }

        public decimal? Pending_balance { get; set; }

        public int? Payment_record_id { get; set; }

        public int? Claim_id { get; set; }

    }
}
