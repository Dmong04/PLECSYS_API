using APPLICATION.Handlers;
using APPLICATION.Use_cases.InvoiceHistories_case;
using FastEndpoints;
using PLECSYS_PROTOTYPE_API.API_response;

namespace PLECSYS_PROTOTYPE_API.Endpoints.InvoiceHistories
{
    public class GetAllInvoiceHistoriesByUserAndCompanyId(InvoiceHistoryHandler _handler) : Endpoint<FindHistoriesRequest, List<InvoiceHistoryResponse>>
    {
        public override void Configure()
        {
            Post("api/v1/plecsys/invoice/history/all");
            AllowAnonymous();
        }

        public override async Task HandleAsync(FindHistoriesRequest request, CancellationToken ct)
        {
            try
            {
                var invoiceHistories = await _handler.GetAllInvoiceHistoriesByUserAndCompanyId(request);

                await ResponseBuilder.BuildResponse<List<InvoiceHistoryResponse>>(invoiceHistories.Data, invoiceHistories.Success,
                    invoiceHistories.Message, HttpContext, StatusCodes.Status200OK, ct);
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
