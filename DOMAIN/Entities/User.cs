using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DOMAIN.Entities
{
    public class User
    {
        [Key]
        public required string Email { get; set; }

        public required string Name { get; set; }

        public required string First_lastname { get; set; }

        public required string Second_lastname { get; set; }

        public required string Phone { get; set; }

        public string? Password { get; set; }

        public DateTime? Created_at { get; set; }

        [JsonIgnore]
        public ICollection<UserCompany>? Linked_companies { get; set; }

        [JsonIgnore]
        public ICollection<SmartFlow>? Linked_processes { get; set; }

        [JsonIgnore]
        public SmartFlow? Approved_Process { get; set; }

        [JsonIgnore]
        public ICollection<Invoice>? Invoices { get; set; }

        [JsonIgnore]
        public ICollection<SaleOrder>? Orders { get; set; }

        [JsonIgnore]
        public ICollection<Claim>? Claims { get; set; }

        [JsonIgnore]
        public ICollection<InvoiceHistory>? Invoice_histories { get; set; }
    }
}
