using DOMAIN;
using DOMAIN.Entities;
using DOMAIN.Interfaces;
using INFRASTRUCTURE.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INFRASTRUCTURE.Repositories
{
    public class PaymentRecordRepository(AppDBContext _ctx) : IPaymentRecordRepository
    {
        public async Task<PaymentRecord> CreatePaymentRecord(PaymentRecord paymentRecord)
        {
            var created = await _ctx.PaymentRecords.AddAsync(paymentRecord);
            await _ctx.SaveChangesAsync();
            var success = await _ctx.PaymentRecords.Include(r => r.Source).Include(r => r.Currency)
                .Include(r => r.Payment_method).FirstOrDefaultAsync(r => r.Payment_record_id == created.Entity.Payment_record_id);
            return success;
        }

        public async Task<List<PaymentRecord>> GetAllPaymentRecords()
        {
            return await _ctx.PaymentRecords.Include(r => r.Source).Include(r => r.Currency)
                .Include(r => r.Payment_method).ToListAsync();
        }

        public async Task<PaymentRecord> GetPaymentRecordsById(int payment_record_id)
        {
            return await _ctx.PaymentRecords.Include(r => r.Source).Include(r => r.Currency)
                .Include(r => r.Payment_method).FirstOrDefaultAsync(r => r.Payment_record_id == payment_record_id);
        }
    }
}
