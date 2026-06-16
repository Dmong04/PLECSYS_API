using APPLICATION.Handlers;
using APPLICATION.Use_cases.Claims_case;
using APPLICATION.Use_cases.InvoiceHistories_case;
using APPLICATION.Use_cases.Invoices_case;
using DOMAIN.Entities;
using FastEndpoints;
using PLECSYS_PROTOTYPE_API.API_response;

namespace PLECSYS_PROTOTYPE_API.Endpoints.InvoiceHistories
{
    public class GetInvoicePaymentHistories(InvoiceHistoryHandler _handler) : EndpointWithoutRequest<List<InvoiceHistoryResponse>>
    {
        public override void Configure()
        {
            Get("api/v1/plecsys/invoice/payment/history/{invoiceId}");
            AllowAnonymous();
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            try
            {
                int invoiceId = Route<int>("invoiceId");

                var histories = await _handler.GetInvoicePaymentHistories(invoiceId);

                await ResponseBuilder.BuildResponse<List<InvoiceHistoryResponse>>(histories.Data, histories.Success,
                        histories.Message, HttpContext, StatusCodes.Status200OK, ct);
                return;
            }
            catch (Exception ex)
            {
                await ResponseBuilder.BuildResponse<List<InvoiceHistoryResponse>>(null, false,
                    "Ha ocurrido un error en el proceso: " + ex.Message, HttpContext, StatusCodes.Status500InternalServerError, ct);
            }
        }
    }
}
