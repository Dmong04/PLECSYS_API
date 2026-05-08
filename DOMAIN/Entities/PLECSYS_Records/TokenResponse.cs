using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DOMAIN.Entities.PLECSYS_Records
{
    public class TokenResponse
    {
        [JsonPropertyName("access_token")]
        public string? AccessToken { get; set; }

        [JsonPropertyName("token_type")]
        public string? TokenType { get; set; }

        [JsonPropertyName("expires_in")]
        public double ExpiresIn { get; set; }

        [JsonPropertyName("userId")]
        public string? UserId { get; set; }
        [JsonPropertyName("email")]
        public string? Email { get; set; }
        [JsonPropertyName("userName")]
        public string? UserName { get; set; }
        [JsonPropertyName("linked_companies")]
        public ICollection<CompanyOption>? Linked_companies { get; set; }
        [JsonPropertyName("linked_processes")]
        public ICollection<SmartFlowOption>? Linked_processes { get; set; }
    }

    public class CompanyOption
    {
        public int company_id { get; set; }
        public string company_name { get; set; } = string.Empty;
    }

    public class SmartFlowOption
    {
        public int smartFlow_id { get; set; }

        public string? smartflow_name { get; set; }

        public string? first_step_id { get; set; }

        public string? first_step_name { get; set; }

        public string? next_step_id { get; set; }

        public string? next_step_name { get; set; }

        public string? approver { get; set; }
    }
}
