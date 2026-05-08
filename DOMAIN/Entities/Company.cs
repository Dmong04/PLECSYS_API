using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DOMAIN.Entities
{
    public class Company
    {
        public int Company_id { get; set; }

        public required string Company_name { get; set; }

        public required string Address { get; set; }

        public required string Phone { get; set; }

        public required string Email { get; set; }

        [JsonIgnore]
        public ICollection<UserCompany>? Company_users { get; set; }

        [JsonIgnore]
        public ICollection<Supplier>? Suppliers { get; set; }

        [JsonIgnore]
        public ICollection<SaleOrder>? Orders { get; set; }

        [JsonIgnore]
        public ICollection<Invoice>? Charged_invoices { get; set; }

        [JsonIgnore]
        public ICollection<Invoice>? Sell_invoices { get; set; }
    }
}
