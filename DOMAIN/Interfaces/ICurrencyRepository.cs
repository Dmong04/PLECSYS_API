using DOMAIN.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DOMAIN.Interfaces
{
    public interface ICurrencyRepository
    {
        Task<List<Currency>> GetAllCurrencies();

        Task<Currency> GetCurrencyById(int currency_id);

        Task<Currency> CreateCurrency(Currency currency);
    }
}
