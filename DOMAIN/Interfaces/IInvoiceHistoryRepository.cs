using DOMAIN.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DOMAIN.Interfaces
{
    public interface IInvoiceHistoryRepository
    {
        Task<List<InvoiceHistory>> GetAllInvoiceHistoriesByUserAndCompanyId(string email, int companyId);

        Task<InvoiceHistory> CreateHistory(InvoiceHistory history);

        Task<List<InvoiceHistory>> GetHistoriesByInvoiceId(int invoice_id);

        Task<InvoiceHistory?> GetInvoiceHistoriesById(int invoice_history_id);

        Task<List<InvoiceHistory>> GetInvoicePaymentHistories(int invoice_id);

        Task<List<InvoiceHistory>> GetInvoiceClaimHistories(int invoice_id);

    }
}
