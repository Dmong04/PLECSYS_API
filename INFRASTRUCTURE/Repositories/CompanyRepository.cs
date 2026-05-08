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
    public class CompanyRepository(AppDBContext _ctx) : ICompanyRepository
    {
        public async Task<Company> CreateCompany(Company company)
        {
            var new_company = await _ctx.Companies.AddAsync(company);
            await _ctx.SaveChangesAsync();
            var success = new Company()
            {
                Company_id = new_company.Entity.Company_id,
                Company_name = new_company.Entity.Company_name,
                Address = new_company.Entity.Address,
                Phone = new_company.Entity.Phone,
                Email = new_company.Entity.Email
            };
            return success;
        }

        public async Task<List<Company>> GetAllCompanies()
        {
            return await _ctx.Companies.ToListAsync();

        }

        public async Task<Company> GetCompanyById(int company_id)
        {
            return await _ctx.Companies.FindAsync(company_id);
        }

        public async Task<Company> GetCompanyByName(string company_name)
        {
            return await _ctx.Companies.FirstOrDefaultAsync(c => c.Company_name == company_name);
        }
    }
}
