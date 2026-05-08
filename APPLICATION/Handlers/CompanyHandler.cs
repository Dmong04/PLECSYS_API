using APPLICATION.Use_cases.Companies_case;
using DOMAIN.Entities;
using DOMAIN.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APPLICATION.Handlers
{
    public class CompanyHandler(ICompanyRepository service)
    {
        public async Task<Response<CompanyResponse>> CreateCompany(CompanyRequest request)
        {
            try
            {
                var found = await this.GetCompanyByName(request.Company_name);
                if (found.Success)
                {
                    return new Response<CompanyResponse>()
                    {
                        Data = found.Data,
                        Success = false,
                        Message = found.Message,
                    };
                }
                var new_company = new Company()
                {
                    Company_name = request.Company_name,
                    Address = request.Address,
                    Phone = request.Phone,
                    Email = request.Email
                };

                var create = await service.CreateCompany(new_company);
                if (create is not null)
                {
                    var success = new CompanyResponse()
                    {
                        Company_id = create.Company_id,
                        Company_name = create.Company_name,
                        Address = create.Address,
                        Phone = create.Phone,
                        Email = create.Email
                    };

                    var response = new Response<CompanyResponse>()
                    {
                        Data = success,
                        Success = true,
                        Message = "Compañia ingresada con éxito"
                    };
                    return response;
                }

                return new Response<CompanyResponse>()
                {
                    Data = null,
                    Success = false,
                    Message = "Ha ocurrido un error al ingresar la compañía"
                };
            } catch (Exception ex)
            {
                return new Response<CompanyResponse>()
                {
                    Data = null,
                    Success = false,
                    Message = "Ha ocurrido un error en el proceso: " + ex.Message
                };
            }
        }

        public async Task<Response<CompanyResponse>> GetCompanyByName(string company_name)
        {
            try
            {
                var found = await service.GetCompanyByName(company_name);
                if (found is null)
                {
                    var failed = new Response<CompanyResponse>()
                    {
                        Data = null,
                        Success = false,
                        Message = "No se ha encontrado la compañía en el servidor"
                    };

                    return failed;
                }
                var success = new CompanyResponse()
                {
                    Company_id = found.Company_id,
                    Company_name = found.Company_name,
                    Address = found.Address,
                    Phone = found.Phone,
                    Email = found.Email
                };
                var response = new Response<CompanyResponse>()
                {
                    Data = success,
                    Success = true,
                    Message = "Se encontró la compañía en el servidor"
                };
                return response;
            } catch (Exception ex)
            {
                return new Response<CompanyResponse>()
                {
                    Data = null,
                    Success = false,
                    Message = "Ha ocurrido un error en la solicitud: " + ex.Message
                };
            }
        }

        public async Task<Response<List<CompanyResponse>>> GetAllCompanies()
        {
            try
            {
                var companies = await service.GetAllCompanies();
                if (companies is null || !companies.Any())
                {
                    return new Response<List<CompanyResponse>>()
                    {
                        Data = null,
                        Success = false,
                        Message = "No se encuentran compañías registradas en el servidor"
                    };
                }

                var selected = companies.Select(c => new CompanyResponse
                {
                    Company_id = c.Company_id,
                    Company_name = c.Company_name,
                    Address = c.Address,
                    Phone = c.Phone,
                    Email = c.Email
                }).ToList();

                return new Response<List<CompanyResponse>>()
                {
                    Data = selected,
                    Success = true,
                    Message = "Se obtuvo el registro de compañías de manera exitosa"
                };
            } catch (Exception ex)
            {
                return new Response<List<CompanyResponse>>()
                {
                    Data = null,
                    Success = false,
                    Message = "Ha ocurrido un error: " + ex.Message
                };
            }
        }
    }
}
