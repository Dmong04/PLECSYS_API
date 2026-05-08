using APPLICATION.Handlers.GPS;
using APPLICATION.Use_cases.GPS.ItineraryRoutes_case.Registry;
using APPLICATION.Use_cases.Invoices_case;
using FastEndpoints;
using PLECSYS_PROTOTYPE_API.API_response;
using System.Threading;

namespace PLECSYS_PROTOTYPE_API.Endpoints.GPS.ItineraryRoutes
{
    public class GetItineraryRoutes(ItineraryRouteHandler _handler) : EndpointWithoutRequest<List<ItineraryRouteResponse>>
    {
        public override void Configure()
        {
            Get("api/v1/plecsys/gps/itinerary/route/seller/id/{seller_id}");
            AllowAnonymous();
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            try
            {
                var seller_id = Route<string>("seller_id");
                var itineraries = await _handler.GetItineraryRoutesBySellerId(seller_id);
                if (itineraries.Data is null)
                    await ResponseBuilder.BuildResponse(itineraries.Data, itineraries.Success,
                    itineraries.Message, HttpContext, StatusCodes.Status400BadRequest, ct);
                await ResponseBuilder.BuildResponse(itineraries.Data, itineraries.Success,
                    itineraries.Message, HttpContext, StatusCodes.Status200OK, ct);
            }
            catch (Exception ex)
            {
                await ResponseBuilder.BuildResponse<List<ItineraryRouteResponse>>(null, false,
                    "Ha ocurrido un error durante el proceso, intente nuevamente más tarde: " + ex.Message,
                    HttpContext, StatusCodes.Status500InternalServerError, ct);
            }
        }
    }
}
