using APPLICATION.Handlers;
using APPLICATION.Use_cases.Companies_case;
using FastEndpoints;
using PLECSYS_PROTOTYPE_API.API_response;

namespace PLECSYS_PROTOTYPE_API.Endpoints.Company
{
    public class CreateCompany(CompanyHandler _handler) : Endpoint<CompanyRequest, CompanyResponse>
    {
        public override void Configure()
        {
            Post("api/v1/plecsys/company/create");
            AllowAnonymous();
        }

        public override async Task HandleAsync(CompanyRequest request, CancellationToken ct)
        {
            try
            {
                var created = await _handler.CreateCompany(request);
                if (!created.Success)
                {
                    await ResponseBuilder.BuildResponse<CompanyResponse>(created.Data, false,
                        created.Message, HttpContext,
                        StatusCodes.Status400BadRequest, ct);
                    return;
                }
                await ResponseBuilder.BuildResponse<CompanyResponse>(created.Data, true,
                    created.Message, HttpContext, StatusCodes.Status200OK, ct);
            }
            catch (Exception ex)
            {
                await ResponseBuilder.BuildResponse<CompanyResponse>(null, false,
                        "Ha ocurrido un error en el proceso:" + ex.Message, HttpContext, StatusCodes.Status500InternalServerError, ct);
            }
        }
    }
}
