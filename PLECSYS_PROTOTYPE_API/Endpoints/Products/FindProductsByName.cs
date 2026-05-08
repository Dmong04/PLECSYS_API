using APPLICATION.Handlers;
using APPLICATION.Use_cases.Products_case;
using FastEndpoints;
using PLECSYS_PROTOTYPE_API.API_response;

namespace PLECSYS_PROTOTYPE_API.Endpoints.Products
{
    public class FindProductsByName(ProductHandler _handler) : EndpointWithoutRequest<List<ProductResponse>>
    {
        public override void Configure()
        {
            Get("api/v1/plecsys/product/search/{product_name}");
            AllowAnonymous();
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            try
            {
                var product_name = Route<string>("product_name");
                var products = await _handler.GetProductsByName(product_name);
                if (products?.Data.Count is 0)
                    await ResponseBuilder.BuildResponse<List<ProductResponse>>(products.Data, products.Success,
                        products.Message, HttpContext, StatusCodes.Status404NotFound, ct);
                await ResponseBuilder.BuildResponse<List<ProductResponse>>(products.Data, products.Success,
                        products.Message, HttpContext, StatusCodes.Status200OK, ct);
            }
            catch (Exception ex)
            {
                await ResponseBuilder.BuildResponse<List<ProductResponse>>(null, false,
                        $"Hubo un error al procesar la solicitud: {ex.Message}",
                        HttpContext, StatusCodes.Status500InternalServerError, ct);
            }
        }
    }
}
