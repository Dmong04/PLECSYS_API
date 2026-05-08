using APPLICATION.Handlers.GPS;
using APPLICATION.Use_cases.GPS.ItineraryRoutes_case.Compare;
using FastEndpoints;
using PLECSYS_PROTOTYPE_API.API_response;

namespace PLECSYS_PROTOTYPE_API.Endpoints.GPS.ItineraryRoutes
{
    public class CompareItineraryWithTrack(ItineraryRouteHandler _handler) : EndpointWithoutRequest<CompareItineraryResponse>
    {
        public override void Configure()
        {
            Get("api/v1/plecsys/gps/itinerary/compare/{itinerary_id}");
            AllowAnonymous();
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            try
            {
                var itinerary_id = Route<string>("itinerary_id");

                var fromStr = Query<string>("from");
                var toStr = Query<string>("to");

                if (!DateTime.TryParse(fromStr, out var parsedFrom))
                {
                    await ResponseBuilder.BuildResponse<CompareItineraryResponse>(
                        null, false,
                        "El parámetro 'from' es requerido y debe ser una fecha válida (yyyy-MM-dd)",
                        HttpContext, StatusCodes.Status400BadRequest, ct);
                    return;
                }

                if (!DateTime.TryParse(toStr, out var parsedTo))
                {
                    await ResponseBuilder.BuildResponse<CompareItineraryResponse>(
                        null, false,
                        "El parámetro 'to' es requerido y debe ser una fecha válida (yyyy-MM-dd)",
                        HttpContext, StatusCodes.Status400BadRequest, ct);
                    return;
                }

                // Transform to UTC
                var from = parsedFrom.ToUniversalTime();
                var to = parsedTo.ToUniversalTime();

                var result = await _handler.CompareItineraryWithTrack(itinerary_id, from, to);

                if (!result.Success && result.Data is null)
                {
                    await ResponseBuilder.BuildResponse<CompareItineraryResponse>(
                        null, result.Success, result.Message,
                        HttpContext, StatusCodes.Status404NotFound, ct);
                    return;
                }

                await ResponseBuilder.BuildResponse(
                    result.Data, result.Success, result.Message,
                    HttpContext, StatusCodes.Status200OK, ct);
            }
            catch (Exception ex)
            {
                await ResponseBuilder.BuildResponse<CompareItineraryResponse>(
                    null, false,
                    "Ha ocurrido un error durante el proceso: " + ex.Message,
                    HttpContext, StatusCodes.Status500InternalServerError, ct);
            }
        }
    }
}
