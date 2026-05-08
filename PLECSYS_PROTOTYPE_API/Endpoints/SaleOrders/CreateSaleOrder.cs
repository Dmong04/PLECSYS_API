using APPLICATION.Handlers;
using APPLICATION.Use_cases.SaleOrders_case;
using FastEndpoints;
using PLECSYS_PROTOTYPE_API.API_response;

namespace PLECSYS_PROTOTYPE_API.Endpoints.SaleOrders
{
    public class CreateSaleOrderDetail(SaleOrderDetailsHandler _handler) : Endpoint<SaleOrdersRequest, SaleOrdersResponse>
    {
        public override void Configure()
        {
            Post("api/v1/plecsys/saleOrder");
            AllowAnonymous();
        }

        public override async Task HandleAsync(SaleOrdersRequest req, CancellationToken ct)
        {
            try
            {
                var request = await _handler.CreateSaleOrder(req);
                await ResponseBuilder.BuildResponse<SaleOrdersResponse>(request.Data, request.Success,
                        request.Message, HttpContext, StatusCodes.Status200OK, ct);
            }
            catch (Exception ex)
            {
                await ResponseBuilder.BuildResponse<SaleOrdersResponse>(null, false,
                    $"Hubo un error al procesar la solicitud: {ex.Message}",
                    HttpContext, StatusCodes.Status500InternalServerError, ct);
            }
        }
    }
}
