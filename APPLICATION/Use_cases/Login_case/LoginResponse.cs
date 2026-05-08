
namespace APPLICATION.Use_cases.Login_case
{
    public class LoginResponse
    {
        public string? access_token { get; set; }

        public string? token_type { get; set; }

        public decimal? expires_in { get; set; }

        public string? userId { get; set; }

        public string? email { get; set; }

        public string? userName { get; set; }

        public ICollection<CompanyOption>? linked_companies { get; set; }

        public ICollection<SmartFlowOption>? linked_processes { get; set; }
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
