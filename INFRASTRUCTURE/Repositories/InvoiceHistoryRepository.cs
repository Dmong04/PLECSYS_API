using DOMAIN.Entities;
using DOMAIN.Interfaces;
using INFRASTRUCTURE.Context;
using Microsoft.EntityFrameworkCore;

namespace INFRASTRUCTURE.Repositories
{
    public class InvoiceHistoryRepository(AppDBContext _ctx) : IInvoiceHistoryRepository
    {
        public async Task<List<InvoiceHistory>> GetAllInvoiceHistoriesByUserAndCompanyId(string email, int companyId)
        {
            return await _ctx.InvoiceHistories
                .AsNoTracking()
                .Include(h => h.Invoice)
                .Include(h => h.User)
                .Include(h => h.PaymentRecord)
                .Include(h => h.Claim)
                .OrderByDescending(h => h.Record_date)
                .Where(h => h.Invoice.User_creator_id == email && h.Invoice.Sell_company_id == companyId)
                .ToListAsync();
        }

        public async Task<InvoiceHistory> CreateHistory(InvoiceHistory history)
        {
            var created = await _ctx.InvoiceHistories.AddAsync(history);
            await _ctx.SaveChangesAsync();
            return created.Entity;
        }

        public async Task<List<InvoiceHistory>> GetHistoriesByInvoiceId(int invoice_id)
        {
            return await _ctx.InvoiceHistories
                .AsNoTracking()
                .Include(h => h.Invoice)
                .Include(h => h.User)
                .Include(h => h.PaymentRecord)
                .Include(h => h.Claim)
                .Where(h => h.Invoice_id == invoice_id)
                .OrderByDescending(h => h.Record_date)
                .ToListAsync();
        }

        public async Task<InvoiceHistory?> GetInvoiceHistoriesById(int invoice_history_id)
        {
            return await _ctx.InvoiceHistories
                .AsNoTracking()
                .Include(h => h.Invoice)
                .Include(h => h.User)
                .Include(h => h.PaymentRecord)
                .Include(h => h.Claim)
                .FirstOrDefaultAsync(h => h.Invoice_history_id == invoice_history_id);
        }

        public async Task<List<InvoiceHistory>> GetInvoicePaymentHistories(int invoice_id)
        {
            return await _ctx.InvoiceHistories
                .AsNoTracking()
                .Include(h => h.Invoice)
                .Include(h => h.User)
                .Include(h => h.PaymentRecord)
                .Include(h => h.Claim)
                .OrderByDescending(h => h.Record_date)
                .ToListAsync();
        }

        public async Task<List<InvoiceHistory>> GetInvoiceClaimHistories(int invoice_id)
        {
            return await _ctx.InvoiceHistories
                .AsNoTracking()
                .Include(h => h.Invoice)
                .Include(h => h.User)
                .Include(h => h.PaymentRecord)
                .Include(h => h.Claim)
                .OrderByDescending(h => h.Record_date)
                .ToListAsync();
        }
    }
}
