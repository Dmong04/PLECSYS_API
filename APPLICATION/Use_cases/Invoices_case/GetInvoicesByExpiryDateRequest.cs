namespace APPLICATION.Use_cases.Invoices_case
{
    public class GetInvoicesByExpiryDateRequest
    {
        public string Email { get; set; } = string.Empty;
        public int CompanyId { get; set; }
        public DateTime ExpiryDate { get; set; }
    }
}
