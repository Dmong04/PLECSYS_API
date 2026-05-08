using APPLICATION.Handlers.GPS;
using APPLICATION.Use_cases.GPS.ItineraryRoutes_case.Registry;
using APPLICATION.Use_cases.Invoices_case;
using FastEndpoints;
using PLECSYS_PROTOTYPE_API.API_response;
using System.Threading;

namespace PLECSYS_PROTOTYPE_API.Endpoints.GPS.ItineraryRoutes
{
    public class RegisterItineraryRoute(ItineraryRouteHandler _handler) : Endpoint<ItineraryRouteRequest, ItineraryRouteResponse>
    {
        public override void Configure()
        {
            Post("api/v1/plecsys/gps/itinerary/route/register");
            AllowAnonymous();
        }

        public override async Task HandleAsync(ItineraryRouteRequest req, CancellationToken ct)
        {
            try
            {
                var result = await _handler.RegisterItineraryRoute(req);
                await ResponseBuilder.BuildResponse(
                    result.Data,
                    result.Success,
                    result.Message,
                    HttpContext,
                    result.Success ? StatusCodes.Status200OK : StatusCodes.Status400BadRequest,
                    ct);
            }
            catch (Exception ex)
            {
                await ResponseBuilder.BuildResponse<ItineraryRouteResponse>(
                    null, false,
                    "Ha ocurrido un error durante el proceso: " + ex.Message,
                    HttpContext, StatusCodes.Status500InternalServerError, ct);
            }
        }
    }
}
