namespace APPLICATION.Use_cases.SignUp_case
{
    public class SignUpRequest
    {
        public required string Email { get; set; }

        public required string Name { get; set; }

        public required string First_lastname { get; set; }

        public required string Second_lastname { get; set; }

        public required string Phone { get; set; }

        public required string Password { get; set; }
    }
}
