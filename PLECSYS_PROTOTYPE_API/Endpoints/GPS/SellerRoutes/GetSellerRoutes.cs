using APPLICATION.Handlers.GPS;
using APPLICATION.Use_cases.GPS.SellerRoutes_case;
using FastEndpoints;
using PLECSYS_PROTOTYPE_API.API_response;

namespace PLECSYS_PROTOTYPE_API.Endpoints.GPS.SellerRoutes
{
    public class GetSellerRoutes(SellerRouteHandler _handler) : EndpointWithoutRequest<List<SellerRouteResponse>>
    {
        public override void Configure()
        {
            Get("api/v1/plecsys/gps/seller/route/seller/id/{seller_id}");
            AllowAnonymous();
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            try
            {
                var seller_id = Route<string>("seller_id");
                var seller_routes = await _handler.GetSellerRoutesBySellerId(seller_id);
                if (seller_routes.Data.Count is 0)
                    await ResponseBuilder.BuildResponse<List<SellerRouteResponse>>(seller_routes.Data, seller_routes.Success,
                        seller_routes.Message, HttpContext, StatusCodes.Status404NotFound, ct);
                await ResponseBuilder.BuildResponse<List<SellerRouteResponse>>(seller_routes.Data, seller_routes.Success,
                        seller_routes.Message, HttpContext, StatusCodes.Status200OK, ct);
            }
            catch (Exception ex)
            {
                await ResponseBuilder.BuildResponse<List<SellerRouteResponse>>(null, false,
                    "Ha ocurrido un error durante el proceso, intente nuevamente más tarde: " + ex.Message,
                    HttpContext, StatusCodes.Status500InternalServerError, ct);
            }
        }
    }
}
