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
    public class PaymentMethodRepository(AppDBContext _ctx) : IPaymentMethodRepository
    {
        public async Task<PaymentMethod> CreatePaymentMethod(PaymentMethod paymentMethod)
        {
            var create = await _ctx.PaymentMethods.AddAsync(paymentMethod);
            await _ctx.SaveChangesAsync();
            var success = new PaymentMethod()
            {
                Payment_method_id = create.Entity.Payment_method_id,
                Payment_method_name = create.Entity.Payment_method_name,
                Payment_method_code = create.Entity.Payment_method_code
            };
            return success;
        }

        public async Task<List<PaymentMethod>> GetAllPaymentPethods()
        {
            return await _ctx.PaymentMethods.ToListAsync();
        }

        public async Task<PaymentMethod> GetPaymentMethodByNameAndCode(string payment_method_name, int payment_method_code)
        {
            return await _ctx.PaymentMethods.FirstOrDefaultAsync(pm => pm.Payment_method_name == payment_method_name
                || pm.Payment_method_code == payment_method_code);
        }

        public async Task<PaymentMethod> GetPaymentMethodById(int payment_method_id)
        {
            return await _ctx.PaymentMethods.FindAsync(payment_method_id);
        }
    }
}
