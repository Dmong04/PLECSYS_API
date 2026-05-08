namespace APPLICATION.Use_cases.SignUp_case
{
    public class SignUpResponse
    {
        public string? Email { get; set; }

        public string? Name { get; set; }

        public string? First_lastname { get; set; }

        public string? Second_lastname { get; set; }

        public string? Phone { get; set; }

        public DateTime? Created_at { get; set; }

        public bool IsCreated { get; set; }
    }
}
