using DOMAIN.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DOMAIN.Interfaces
{
    public interface IPaymentRecordRepository
    {
        Task<List<PaymentRecord>> GetAllPaymentRecords();

        Task<PaymentRecord> GetPaymentRecordsById(int payment_record_id);

        Task<PaymentRecord> CreatePaymentRecord(PaymentRecord paymentRecord);
    }
}
