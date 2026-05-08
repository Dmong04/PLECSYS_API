using APPLICATION.Handlers.GPS;
using APPLICATION.Use_cases.GPS.SellerRoutes_case;
using FastEndpoints;
using PLECSYS_PROTOTYPE_API.API_response;

namespace PLECSYS_PROTOTYPE_API.Endpoints.GPS.SellerRoutes
{
    public class RegisterSellerRoute(SellerRouteHandler _handler) : Endpoint<SellerRouteRequest, SellerRouteResponse>
    {
        public override void Configure()
        {
            Post("api/v1/plecsys/gps/seller/route/register");
            AllowAnonymous();
        }

        public override async Task HandleAsync(SellerRouteRequest request, CancellationToken ct)
        {
            try
            {
                var created = await _handler.RegisterSellerRoute(request);
                if (created is null)
                    await ResponseBuilder.BuildResponse(created.Data, created.Success,
                    created.Message, HttpContext, StatusCodes.Status400BadRequest, ct);
                await ResponseBuilder.BuildResponse(created.Data, created.Success,
                    created.Message, HttpContext, StatusCodes.Status200OK, ct);
            }
            catch (Exception ex)
            {
                await ResponseBuilder.BuildResponse<SellerRouteResponse>(null, false,
                    "Ha ocurrido un error durante el proceso, intente nuevamente más tarde: " + ex.Message,
                    HttpContext, StatusCodes.Status500InternalServerError, ct);
            }
        }
    }
}
