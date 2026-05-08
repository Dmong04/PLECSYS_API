using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APPLICATION.Use_cases.Claims_case
{
    public class ClaimResponse
    {
        public int? Claim_id { get; set; }

        public DateTime? Record_date { get; set; }

        public string? User { get; set; }

        public string? Description { get; set; }

        public int? Invoice { get; set; }

        public decimal? Claim_amount { get; set; }
    }
}
