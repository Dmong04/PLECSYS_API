using DOMAIN;
using DOMAIN.Entities;
using DOMAIN.Interfaces;
using INFRASTRUCTURE.Context;
using Microsoft.EntityFrameworkCore;

namespace INFRASTRUCTURE.Repositories
{
    public class InvoiceRepository(AppDBContext _ctx) : IInvoiceRepository
    {
        public async Task<Invoice> CreateInvoice(Invoice invoice)
        {
            var created = await _ctx.Invoices.AddAsync(invoice);
            await _ctx.SaveChangesAsync();
            var success = await _ctx.Invoices
                .Include(i => i.User)
                .Include(i => i.Sell_company)
                .Include(i => i.Charged_company)
                .Include(i => i.Currency)
                .FirstOrDefaultAsync(i => i.Invoice_id == created.Entity.Invoice_id);

            return success;
        }

        public async Task<List<Invoice>> GetAllInvoices()
        {
            return await _ctx.Invoices.Include(i => i.User).Include(i => i.Sell_company)
                .Include(i => i.Charged_company).Include(i => i.Currency).ToListAsync();
        }

        public async Task<List<Invoice>> GetInvoicesByClient(string email)
        {
            return await _ctx.Invoices.Include(i => i.User).Include(i => i.Sell_company)
                .Include(i => i.Charged_company).Include(i => i.Currency)
                .Where(i => i.User.Email == email)
                .ToListAsync();
        }

        public async Task<Invoice> UpdateInvoice(Invoice invoice)
        {
            _ctx.Invoices.Update(invoice);
            await _ctx.SaveChangesAsync();
            return invoice;
        }

        public async Task<Invoice> GetInvoiceById(int invoice_id)
        {
            return await _ctx.Invoices.Include(i => i.User).Include(i => i.Sell_company)
                .Include(i => i.Charged_company).Include(i => i.Currency)
                .FirstOrDefaultAsync(i => i.Invoice_id == invoice_id);
        }

        public Task<Invoice?> GetInvoiceForPdfAsync(int invoiceId, CancellationToken ct = default) =>
        _ctx.Invoices
            .AsNoTracking()
            .Include(i => i.User)
            .Include(i => i.Sell_company)
            .Include(i => i.Charged_company)
            .Include(i => i.Currency)
            .Include(i => i.Payment_records).ThenInclude(pr => pr.Payment_method)
            .FirstOrDefaultAsync(i => i.Invoice_id == invoiceId, ct);

        public async Task<List<Invoice>> GetInvoicesBySellCompany(int sell_company_id)
        {
            return await _ctx.Invoices.Include(i => i.User).Include(i => i.Sell_company)
            .Include(i => i.Charged_company).Include(i => i.Currency).Include(i => i.Payment_records)
            .ThenInclude(pr => pr.Payment_method).Where(i => i.Sell_company_id == sell_company_id)
            .Where(i => i.Status == "Con reclamo" || i.Status == "Pendiente" || i.Status == "Parcial").ToListAsync();
        }
    }
}
