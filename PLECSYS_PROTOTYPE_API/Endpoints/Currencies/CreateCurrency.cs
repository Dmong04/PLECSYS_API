using APPLICATION.Handlers;
using APPLICATION.Use_cases.Currencies_case;
using FastEndpoints;
using PLECSYS_PROTOTYPE_API.API_response;

namespace PLECSYS_PROTOTYPE_API.Endpoints.Currencies
{
    public class CreateCurrency(CurrencyHandler _handler) : Endpoint<CurrencyRequest, CurrencyResponse>
    {
        public override void Configure()
        {
            Post("api/v1/plecsys/currency/create");
            AllowAnonymous();
        }

        public override async Task HandleAsync(CurrencyRequest request, CancellationToken ct)
        {
            try
            {
                var created = await _handler.CreateCurrency(request);
                if (created.Data is null)
                    await ResponseBuilder.BuildResponse<CurrencyResponse>(created.Data, created.Success, created.Message, 
                        HttpContext, StatusCodes.Status400BadRequest, ct);
                await ResponseBuilder.BuildResponse<CurrencyResponse>(created.Data, created.Success, created.Message,
                    HttpContext, StatusCodes.Status200OK, ct);
            } catch (Exception ex)
            {
                await ResponseBuilder.BuildResponse<CurrencyResponse>(null, false,
                    "Ha habiddo un problema al procesar la solicitud" + ex.Message,
                    HttpContext, StatusCodes.Status500InternalServerError, ct);
            }
        }
    }
}
