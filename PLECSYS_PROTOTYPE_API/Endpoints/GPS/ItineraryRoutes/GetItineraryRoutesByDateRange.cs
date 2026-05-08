using APPLICATION.Handlers.GPS;
using APPLICATION.Use_cases.GPS.ItineraryRoutes_case.Registry;
using FastEndpoints;
using PLECSYS_PROTOTYPE_API.API_response;

namespace PLECSYS_PROTOTYPE_API.Endpoints.GPS.ItineraryRoutes
{
    public class GetItineraryRoutesByDateRange(ItineraryRouteHandler _handler)
    : EndpointWithoutRequest<List<ItineraryRouteResponse>>
    {
        public override void Configure()
        {
            Get("api/v1/plecsys/gps/itinerary/route/seller/date-range");
            AllowAnonymous();
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            try
            {
                var seller_id = Query<string>("sellerId");
                var fromStr = Query<string>("from");
                var toStr = Query<string>("to");

                if (!DateTime.TryParse(fromStr, null,
                    System.Globalization.DateTimeStyles.AdjustToUniversal |
                    System.Globalization.DateTimeStyles.AssumeUniversal, out var parsedFrom))
                {
                    await ResponseBuilder.BuildResponse<List<ItineraryRouteResponse>>(
                        null, false,
                        "El parámetro 'from' es requerido y debe ser una fecha válida (yyyy-MM-ddTHH:mm:ssZ)",
                        HttpContext, StatusCodes.Status400BadRequest, ct);
                    return;
                }

                if (!DateTime.TryParse(toStr, null,
                    System.Globalization.DateTimeStyles.AdjustToUniversal |
                    System.Globalization.DateTimeStyles.AssumeUniversal, out var parsedTo))
                {
                    await ResponseBuilder.BuildResponse<List<ItineraryRouteResponse>>(
                        null, false,
                        "El parámetro 'to' es requerido y debe ser una fecha válida (yyyy-MM-ddTHH:mm:ssZ)",
                        HttpContext, StatusCodes.Status400BadRequest, ct);
                    return;
                }

                var from = parsedFrom.ToUniversalTime();
                var to = parsedTo.ToUniversalTime();

                var result = await _handler.GetItineraryRoutesByDateRange(seller_id, from, to);
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
