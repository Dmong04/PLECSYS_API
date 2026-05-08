using APPLICATION.Handlers.GPS;
using APPLICATION.Use_cases.GPS.ItineraryRoutes_case.Registry;
using FastEndpoints;
using PLECSYS_PROTOTYPE_API.API_response;

namespace PLECSYS_PROTOTYPE_API.Endpoints.GPS.ItineraryRoutes
{
    public class GetItineraryRoutesByOwner(ItineraryRouteHandler _handler)
    : EndpointWithoutRequest<List<ItineraryRouteResponse>>
    {
        public override void Configure()
        {
            Get("api/v1/plecsys/gps/itinerary/route/owner/{owner_email}");
            AllowAnonymous();
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            try
            {
                var owner_email = Route<string>("owner_email");
                var result = await _handler.GetItineraryRoutesByOwner(owner_email);
                await ResponseBuilder.BuildResponse(
                    result.Data,
                    result.Success,
                    result.Message,
                    HttpContext,
                    result.Success ? StatusCodes.Status200OK : StatusCodes.Status404NotFound,
                    ct);
            }
            catch (Exception ex)
            {
                await ResponseBuilder.BuildResponse<List<ItineraryRouteResponse>>(
                    null, false,
                    "Ha ocurrido un error durante el proceso: " + ex.Message,
                    HttpContext, StatusCodes.Status500InternalServerError, ct);
            }
        }
    }
}
