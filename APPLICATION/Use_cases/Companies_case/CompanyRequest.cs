using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APPLICATION.Use_cases.Companies_case
{
    public class CompanyRequest
    {
        public required string Company_name { get; set; }

        public required string Address { get; set; }

        public required string Phone { get; set; }

        public required string Email { get; set; }
    }
}
