using APPLICATION.Handlers.GPS;
using APPLICATION.Use_cases.GPS.ItineraryRoutes_case.Tracking_visits;
using FastEndpoints;
using PLECSYS_PROTOTYPE_API.API_response;

namespace PLECSYS_PROTOTYPE_API.Endpoints.GPS.ItineraryRoutes
{
    public class TrackItineraryRoute(ItineraryRouteHandler _handler) : Endpoint<TargetPointRequest, TrackingItineraryResponse>
    {
        public override void Configure()
        {
            Post("api/v1/plecsys/gps/itinerary/route/target/point/check");
            AllowAnonymous();
        }

        public override async Task HandleAsync(TargetPointRequest request, CancellationToken ct)
        {
            try
            {
                var point_checker = await _handler.TrackItineraryVisits(request);

                switch (point_checker.Data.Status)
                {
                    case "VISITED":
                        await ResponseBuilder.BuildResponse(point_checker.Data, point_checker.Success,
                            point_checker.Message, HttpContext, StatusCodes.Status200OK, ct);
                        break;

                    case "NO_MATCH":
                        await ResponseBuilder.BuildResponse(point_checker.Data, point_checker.Success,
                            point_checker.Message, HttpContext, StatusCodes.Status404NotFound, ct);
                        break;

                    case "NO_ITINERARY":
                        await ResponseBuilder.BuildResponse(point_checker.Data, point_checker.Success,
                            point_checker.Message, HttpContext, StatusCodes.Status404NotFound, ct);
                        break;

                    case "ERROR":
                        await ResponseBuilder.BuildResponse(point_checker.Data, point_checker.Success,
                            point_checker.Message, HttpContext, StatusCodes.Status400BadRequest, ct);
                        break;
                }
            }
            catch (Exception ex)
            {
                await ResponseBuilder.BuildResponse<TrackingItineraryResponse>(null, false,
                            "Estado desconocido en la respuesta del handler: " + ex.Message,
                            HttpContext, StatusCodes.Status500InternalServerError, ct);
            }
        }
    }
}
