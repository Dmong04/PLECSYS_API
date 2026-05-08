using DOMAIN.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DOMAIN.Interfaces
{
    public interface IPaymentMethodRepository
    {
        Task<List<PaymentMethod>> GetAllPaymentPethods();

        Task<PaymentMethod> GetPaymentMethodById(int payment_method_id);

        Task<PaymentMethod> GetPaymentMethodByNameAndCode(string payment_method_name, int payment_method_code);

        Task<PaymentMethod> CreatePaymentMethod(PaymentMethod paymentMethod);
    }
}
