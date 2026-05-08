using APPLICATION.Handlers;
using APPLICATION.Use_cases.Claims_case;
using APPLICATION.Use_cases.PaymentMethods_case;
using FastEndpoints;
using PLECSYS_PROTOTYPE_API.API_response;

namespace PLECSYS_PROTOTYPE_API.Endpoints.Claims
{
    public class CreateClaim(ClaimHandler _handler) : Endpoint<ClaimRequest, ClaimResponse>
    {
        public override void Configure()
        {
            Post("api/v1/plecsys/claim/create");
            AllowAnonymous();
        }

        public override async Task HandleAsync(ClaimRequest request, CancellationToken ct)
        {
            try
            {
                var created = await _handler.CreateClaim(request);
                if (!created.Success)
                {
                    await ResponseBuilder.BuildResponse<ClaimResponse>(created.Data, false,
                    "No se ha creado el reclamo porque este ya existe",
                    HttpContext, StatusCodes.Status400BadRequest, ct);
                }
                await ResponseBuilder.BuildResponse<ClaimResponse>(created.Data, true,
                    "Reclamo creado con éxito",
                    HttpContext, StatusCodes.Status200OK, ct);
            }
            catch (Exception ex)
            {
                await ResponseBuilder.BuildResponse<ClaimResponse>(null, false,
                    "Ha ocurrido un error en el proceso: " + ex.Message, HttpContext, StatusCodes.Status500InternalServerError, ct);
            }
        }
    }
}
