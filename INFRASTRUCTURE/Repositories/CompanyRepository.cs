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
    /// <summary>
    /// Repositorio para la gestión de empresas en la base de datos SQL Server.
    /// Implementa <see cref="ICompanyRepository"/> usando Entity Framework Core.
    /// </summary>
    /// <param name="_ctx">Contexto de base de datos de la aplicación.</param>
    public class CompanyRepository(AppDBContext _ctx) : ICompanyRepository
    {
        /// <summary>
        /// Crea una nueva empresa en la base de datos y retorna el registro creado
        /// con sus datos básicos sin incluir relaciones de navegación.
        /// </summary>
        /// <param name="company">Objeto <see cref="Company"/> con los datos de la empresa a crear.</param>
        /// <returns>La <see cref="Company"/> recién creada con su identificador asignado.</returns>
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

        /// <summary>
        /// Obtiene todas las empresas registradas en el sistema.
        /// </summary>
        /// <returns>Lista de todas las <see cref="Company"/> disponibles.</returns>
        public async Task<List<Company>> GetAllCompanies()
        {
            return await _ctx.Companies.ToListAsync();
        }

        /// <summary>
        /// Obtiene una empresa específica por su identificador único.
        /// </summary>
        /// <param name="company_id">Identificador único de la empresa a buscar.</param>
        /// <returns>
        /// La <see cref="Company"/> correspondiente al identificador,
        /// o <c>null</c> si no existe ninguna empresa con ese identificador.
        /// </returns>
        public async Task<Company> GetCompanyById(int company_id)
        {
            return await _ctx.Companies.FindAsync(company_id);
        }

        /// <summary>
        /// Obtiene una empresa específica por su nombre.
        /// </summary>
        /// <param name="company_name">Nombre de la empresa a buscar.</param>
        /// <returns>
        /// La <see cref="Company"/> que coincide con el nombre proporcionado,
        /// o <c>null</c> si no existe ninguna empresa con ese nombre.
        /// </returns>
        public async Task<Company> GetCompanyByName(string company_name)
        {
            return await _ctx.Companies.FirstOrDefaultAsync(c => c.Company_name == company_name);
        }
    }
}