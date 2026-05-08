using APPLICATION.Handlers;
using APPLICATION.Use_cases.Companies_case;
using FastEndpoints;
using PLECSYS_PROTOTYPE_API.API_response;

namespace PLECSYS_PROTOTYPE_API.Endpoints.Company
{
    public class GetAllCompanies(CompanyHandler _handler) : EndpointWithoutRequest<List<CompanyResponse>>
    {
        public override void Configure()
        {
            Get("api/v1/plecsys/company/all");
            AllowAnonymous();
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            try
            {
                var companies = await _handler.GetAllCompanies();
                if (companies is null)
                {
                    await ResponseBuilder.BuildResponse<List<CompanyResponse>>(null, false,
                        "No hay compañías registradas en el servidor", HttpContext, StatusCodes.Status204NoContent, ct);
                }
                await ResponseBuilder.BuildResponse<List<CompanyResponse>>(companies.Data, true,
                    "Se obtuvo el registro de compañías de manera exitosa", HttpContext, StatusCodes.Status200OK, ct);
            } catch (Exception ex)
            {
                await ResponseBuilder.BuildResponse<List<CompanyResponse>>(null, false,
                        "Ha ocurrido un error en el proceso:" + ex.Message, HttpContext, StatusCodes.Status500InternalServerError, ct);
            }
        }
    }
}
