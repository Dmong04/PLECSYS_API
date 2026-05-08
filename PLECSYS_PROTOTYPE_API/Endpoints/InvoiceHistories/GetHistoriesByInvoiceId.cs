using APPLICATION.Handlers;
using APPLICATION.Use_cases.InvoiceHistories_case;
using FastEndpoints;
using PLECSYS_PROTOTYPE_API.API_response;

namespace PLECSYS_PROTOTYPE_API.Endpoints.InvoiceHistories
{
    public class GetInvoiceHistoriesByInvoiceId(InvoiceHistoryHandler _handler)
        : EndpointWithoutRequest<List<InvoiceHistoryResponse>>
    {
        public override void Configure()
        {
            Get("api/v1/plecsys/invoice/history/invoice/{invoiceId}");
            AllowAnonymous();
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            try
            {
                int invoiceId = Route<int>("invoiceId");

                var histories = await _handler.GetInvoiceHistoriesByInvoiceId(invoiceId);

                if (histories is null || histories.Data is null || !histories.Data.Any())
                {
                    await ResponseBuilder.BuildResponse<List<InvoiceHistoryResponse>>(
                        null,
                        false,
                        "La factura no posee registros de historial",
                        HttpContext,
                        StatusCodes.Status200OK,
                        ct
                    );
                    return;
                }

                await ResponseBuilder.BuildResponse<List<InvoiceHistoryResponse>>(
                    histories.Data,
                    true,
                    "Historial de la factura obtenido exitosamente",
                    HttpContext,
                    StatusCodes.Status200OK,
                    ct
                );
            }
            catch (Exception ex)
            {
                await ResponseBuilder.BuildResponse<List<InvoiceHistoryResponse>>(
                    null,
                    false,
                    "Ha ocurrido un error en el proceso: " + ex.Message,
                    HttpContext,
                    StatusCodes.Status500InternalServerError,
                    ct
                );
            }
        }
    }
}

