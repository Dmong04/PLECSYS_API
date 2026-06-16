using DOMAIN.Entities;

namespace DOMAIN.Interfaces
{
    public interface IInvoiceRepository
    {
        Task<List<Invoice>> GetAllInvoices(string email, int companyId);

        Task<List<Invoice>> GetInvoicesBySellCompany(int sell_company_id);

        Task<Invoice> GetInvoiceById(int invoice_id);

        Task<Invoice> CreateInvoice(Invoice invoice);

        Task<Invoice> UpdateInvoice(Invoice invoice);

        Task<List<Invoice>> GetInvoicesByClient(string email);

        Task<Invoice?> GetInvoiceForPdfAsync(int invoiceId, CancellationToken ct = default);

        Task<List<Invoice>> GetInvoicesByExpiryDateAndUserEmail(string email, int companyId, DateTime expiryDate);
    }
}
