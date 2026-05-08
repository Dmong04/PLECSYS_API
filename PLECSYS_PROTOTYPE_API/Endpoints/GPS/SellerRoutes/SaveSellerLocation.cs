using APPLICATION.Handlers.GPS;
using APPLICATION.Use_cases.GPS.ItineraryRoutes_case.Tracking_visits;
using APPLICATION.Use_cases.GPS.TrackingLocation_case;
using FastEndpoints;
using PLECSYS_PROTOTYPE_API.API_response;

namespace PLECSYS_PROTOTYPE_API.Endpoints.GPS.SellerRoutes
{
    public class SaveSellerLocation(SellerLocationTrackingHandler _locationHandler, ItineraryRouteHandler _itineraryHandler)
       : Endpoint<SaveLocationRequest, SaveLocationResponse>
    {
        public override void Configure()
        {
            Post("api/v1/plecsys/gps/seller/location");
            AllowAnonymous();
        }

        public override async Task HandleAsync(SaveLocationRequest request, CancellationToken ct)
        {
            try
            {
                // 1. Save seller's location
                var result = await _locationHandler.SaveLocationAsync(request);

                if (!result.Success)
                {
                    await ResponseBuilder.BuildResponse(
                        result.Data,
                        result.Success,
                        result.Message,
                        HttpContext,
                        StatusCodes.Status400BadRequest,
                        ct);
                    return;
                }

                // 2. Automatically check if you are within any point of the itinerary
                var trackRequest = new TargetPointRequest
                {
                    Seller_id = request.SellerId,
                    Timestamp = DateTime.UtcNow,
                    Reference_name = string.Empty,
                    Location = new TargetLocationRequest
                    {
                        Coordinates = [request.Longitude, request.Latitude]
                    }
                };

                await _itineraryHandler.TrackItineraryVisits(trackRequest);

                // 3. Respond with the save result
                // (regardless of whether a point was found or not)
                await ResponseBuilder.BuildResponse(
                    result.Data,
                    result.Success,
                    result.Message,
                    HttpContext,
                    StatusCodes.Status200OK,
                    ct);
            }
            catch (Exception ex)
            {
                await ResponseBuilder.BuildResponse<SaveLocationResponse>(
                    null, false,
                    "Ha ocurrido un error al guardar la ubicación: " + ex.Message,
                    HttpContext,
                    StatusCodes.Status500InternalServerError,
                    ct);
            }
        }
    }
}
