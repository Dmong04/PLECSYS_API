using APPLICATION.Handlers;
using APPLICATION.Use_cases.InvoiceHistories_case;
using APPLICATION.Use_cases.PaymentMethods_case;
using FastEndpoints;
using PLECSYS_PROTOTYPE_API.API_response;

namespace PLECSYS_PROTOTYPE_API.Endpoints.InvoiceHistories
{
    public class GetAllInvoiceHistories(InvoiceHistoryHandler _handler): EndpointWithoutRequest<List<InvoiceHistoryResponse>>
    {
        public override void Configure()
        {
            Get("api/v1/plecsys/invoice/history/all");
            AllowAnonymous();
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            try
            {
                var invoiceHistories = await _handler.GetAllInvoiceHistories();

                if (invoiceHistories is null || invoiceHistories.Data is null || !invoiceHistories.Data.Any())
                {
                    await ResponseBuilder.BuildResponse<List<InvoiceHistoryResponse>>(null, false,
                        "No hay historial de facturas registrados en el servidor", HttpContext, StatusCodes.Status200OK, ct);
                    return;
                }

                await ResponseBuilder.BuildResponse<List<InvoiceHistoryResponse>>(invoiceHistories.Data, true,
                    "Se obtuvo el listado del historial de facturas exitosamente", HttpContext, StatusCodes.Status200OK, ct);
            }
            catch (Exception ex)
            {
                await ResponseBuilder.BuildResponse<List<InvoiceHistoryResponse>>(null, false,
                    "Ha ocurrido un error en el proceso: " + ex.Message, HttpContext, StatusCodes.Status500InternalServerError, ct);
            }
        }
    }
}
