using APPLICATION.Handlers;
using APPLICATION.Use_cases.Currencies_case;
using FastEndpoints;
using PLECSYS_PROTOTYPE_API.API_response;

namespace PLECSYS_PROTOTYPE_API.Endpoints.Currencies
{
    public class GetAllCurrencies(CurrencyHandler _handler) : EndpointWithoutRequest<List<CurrencyResponse>>
    {
        public override void Configure()
        {
            Get("api/v1/plecsys/currency/all");
            AllowAnonymous();
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            try
            {
                var currencies = await _handler.GetAllCurrencies();
                if (currencies.Data.Count is 0)
                    await ResponseBuilder.BuildResponse<List<CurrencyResponse>>(currencies.Data, currencies.Success, currencies.Message,
                    HttpContext, StatusCodes.Status200OK, ct);
                await ResponseBuilder.BuildResponse<List<CurrencyResponse>>(currencies.Data, currencies.Success, currencies.Message,
                    HttpContext, StatusCodes.Status200OK, ct);
            } catch (Exception ex)
            {
                await ResponseBuilder.BuildResponse<List<CurrencyResponse>>(null, false,
                    "Ha habiddo un problema al procesar la solicitud" + ex.Message,
                    HttpContext, StatusCodes.Status500InternalServerError, ct);
            }
        }
    }
}
