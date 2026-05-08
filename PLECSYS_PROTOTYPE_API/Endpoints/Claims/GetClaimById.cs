using APPLICATION.Handlers;
using APPLICATION.Use_cases.Claims_case;
using APPLICATION.Use_cases.InvoiceHistories_case;
using FastEndpoints;
using PLECSYS_PROTOTYPE_API.API_response;

namespace PLECSYS_PROTOTYPE_API.Endpoints.Claims
{
    public class GetClaimById(ClaimHandler _handler) : EndpointWithoutRequest<List<ClaimResponse>>
    {
        public override void Configure()
        {
            Get("api/v1/plecsys/claim/{claim_id}");
            AllowAnonymous();
        }
        public override async Task HandleAsync(CancellationToken ct)
        {
            try
            {
                var claim_id = Route<int>("claim_id");
                var claim = await _handler.GetClaimById(claim_id);
                if (claim.Data is null)
                {
                    await ResponseBuilder.BuildResponse<ClaimResponse>(null, false,
                    "No se encontró el reclamo con el ID proporcionado", HttpContext, StatusCodes.Status200OK, ct);
                    return;
                }
                await ResponseBuilder.BuildResponse<ClaimResponse>(claim.Data, true,
                    "Se obtuvo el reclamo exitosamente", HttpContext, StatusCodes.Status200OK, ct);
            }
            catch (Exception ex)
            {
                await ResponseBuilder.BuildResponse<ClaimResponse>(null, false,
                    "Ha ocurrido un error en el proceso: " + ex.Message, HttpContext, StatusCodes.Status500InternalServerError, ct);
            }
        }
    }
}
