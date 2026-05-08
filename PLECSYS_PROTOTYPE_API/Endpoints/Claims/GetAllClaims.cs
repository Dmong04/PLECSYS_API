using APPLICATION.Handlers;
using APPLICATION.Use_cases.Claims_case;
using APPLICATION.Use_cases.InvoiceHistories_case;
using FastEndpoints;
using PLECSYS_PROTOTYPE_API.API_response;

namespace PLECSYS_PROTOTYPE_API.Endpoints.Claims
{
    public class GetAllClaims(ClaimHandler _handler) : EndpointWithoutRequest<List<ClaimResponse>>
    {
        public override void Configure()
        {
            Get("api/v1/plecsys/claim/all");
            AllowAnonymous();
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            try
            {
                var claims = await _handler.GetAllClaims();

                if (claims is null || claims.Data is null || !claims.Data.Any())
                {
                    await ResponseBuilder.BuildResponse<List<ClaimResponse>>(null, false,
                        "No hay reclamos registrados en el servidor", HttpContext, StatusCodes.Status200OK, ct);
                    return;
                }

                await ResponseBuilder.BuildResponse<List<ClaimResponse>>(claims.Data, true,
                    "Se obtuvo el listado de reclamos exitosamente", HttpContext, StatusCodes.Status200OK, ct);
            }
            catch (Exception ex)
            {
                await ResponseBuilder.BuildResponse<List<ClaimResponse>>(null, false,
                    "Ha ocurrido un error en el proceso: " + ex.Message, HttpContext, StatusCodes.Status500InternalServerError, ct);
            }
        }
    }
}
