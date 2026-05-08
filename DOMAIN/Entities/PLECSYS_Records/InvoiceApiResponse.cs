namespace DOMAIN.Entities.PLECSYS_Records
{

    public class InvoiceApiResponse
    {
        public List<InvoiceResponse> Data { get; set; } = new();
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
    }

    public class InvoiceResponse
    {
        public int Invoice_id { get; set; }

        public int Consecutive { get; set; }

        public decimal Total_voucher { get; set; }

        public Company? Sell_company { get; set; }

        public Company? Charged_company { get; set; }

        public Currency? Currency { get; set; }

        public string? Status { get; set; }

        public decimal? Pending_balance { get; set; }

        public DateTime? Created_at { get; set; }
    }
}
