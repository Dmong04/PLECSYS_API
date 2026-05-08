using APPLICATION.Handlers;
using APPLICATION.Use_cases.Invoices_case;
using FastEndpoints;
using PLECSYS_PROTOTYPE_API.API_response;

namespace PLECSYS_PROTOTYPE_API.Endpoints.Invoices
{
    public class GetInvoiceBySellCompany(InvoiceHandler _handler) : EndpointWithoutRequest<List<InvoiceResponse>>
    {
        public override void Configure()
        {
            Get("api/v1/plecsys/invoice/sell-company/{sell_company_id}");
            AllowAnonymous();
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            try
            {
                var sell_company_id = Route<int>("sell_company_id");
                if (sell_company_id is 0)
                {
                    await ResponseBuilder.BuildResponse<List<InvoiceResponse>>(null, false,
                        "El id de la compañía no se proporcionó", HttpContext, StatusCodes.Status400BadRequest, ct);
                    return;
                }

                var response = await _handler.GetInvoicesBySellCompany(sell_company_id);
                if (response is null || response?.Data?.Count is 0)
                {
                    await ResponseBuilder.BuildResponse<List<InvoiceResponse>>(response?.Data, response.Success,
                        response.Message, HttpContext, StatusCodes.Status200OK, ct);
                    return;
                }

                await ResponseBuilder.BuildResponse<List<InvoiceResponse>>(response?.Data, response.Success,
                        response.Message, HttpContext, StatusCodes.Status200OK, ct);
            }
            catch (Exception ex)
            {
                await ResponseBuilder.BuildResponse<List<InvoiceResponse>>(null, false,
                    "Ha ocurrido un error durante el proceso, intente nuevamente más tarde: " + ex.Message,
                    HttpContext, StatusCodes.Status500InternalServerError, ct);
            }
        }
    }
}
