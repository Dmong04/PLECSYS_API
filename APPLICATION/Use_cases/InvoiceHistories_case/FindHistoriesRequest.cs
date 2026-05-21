namespace APPLICATION.Use_cases.InvoiceHistories_case
{
    public class FindHistoriesRequest
    {
        public required string Email { get; set; }

        public int CompanyId { get; set; }
    }
}
