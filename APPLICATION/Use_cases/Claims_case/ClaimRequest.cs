using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APPLICATION.Use_cases.Claims_case
{
    public class ClaimRequest
    {
        public required DateTime Record_date { get; set; }

        public required string User_id { get; set; }

        public required string Description { get; set; }

        public required int Invoice_id { get; set; }

        public required decimal Claim_amount { get; set; }
    }
}
