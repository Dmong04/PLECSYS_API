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
    public class CurrencyRepository(AppDBContext _ctx) : ICurrencyRepository
    {
        public async Task<Currency> CreateCurrency(Currency currency)
        {
            var created = await _ctx.Currencies.AddAsync(currency);
            await _ctx.SaveChangesAsync();
            var success = await _ctx.Currencies.FirstOrDefaultAsync(c => c.Currency_id == created.Entity.Currency_id);
            return success;
        }

        public async Task<List<Currency>> GetAllCurrencies()
        {
            return await _ctx.Currencies.ToListAsync();
        }

        public async Task<Currency> GetCurrencyById(int currency_id)
        {
            return await _ctx.Currencies.FindAsync(currency_id);
        }
    }
}
