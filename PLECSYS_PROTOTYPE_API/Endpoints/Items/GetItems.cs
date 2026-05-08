using APPLICATION.Use_cases.Items_case;
using DOMAIN.Entities.PLECSYS_Records.Items;
using FastEndpoints;
using PLECSYS_PROTOTYPE_API.API_response;

namespace PLECSYS_PROTOTYPE_API.Endpoints.Items
{
    public class GetItems(IItemUseCase _useCase) : Endpoint<ItemsRequest, ItemApiResponse>
    {
        public override void Configure()
        {
            Post("api/Invoice/GetItemsFromApp");
            AllowAnonymous();
        }

        public override async Task HandleAsync(ItemsRequest req, CancellationToken ct)
        {
            var token = HttpContext.Request.Headers.Authorization.ToString().Replace("Bearer ", "");
            var response = await _useCase.ExecuteAsync(req, token);
            await ResponseBuilder.BuildResponse(response.Data, response.Success, response.Message,
                HttpContext, StatusCodes.Status200OK, ct);
        }
    }
}
