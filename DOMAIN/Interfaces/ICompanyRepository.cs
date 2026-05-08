using DOMAIN.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DOMAIN.Interfaces
{
    public interface ICompanyRepository
    {
        Task<List<Company>> GetAllCompanies();

        Task<Company> GetCompanyById(int company_id);
        Task<Company> GetCompanyByName(string company_name);

        Task<Company> CreateCompany(Company company);
    }
}
