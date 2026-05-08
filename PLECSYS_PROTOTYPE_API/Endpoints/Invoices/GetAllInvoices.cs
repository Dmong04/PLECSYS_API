using APPLICATION.Handlers;
using APPLICATION.Use_cases.Invoices_case;
using FastEndpoints;
using PLECSYS_PROTOTYPE_API.API_response;

namespace PLECSYS_PROTOTYPE_API.Endpoints.Invoices
{
    public class GetAllInvoices(InvoiceHandler handler) : EndpointWithoutRequest<List<InvoiceResponse>>
    {
        public override void Configure()
        {
            Get("api/v1/plecsys/invoice/all");
            AllowAnonymous();
        }

        public override async Task HandleAsync(CancellationToken cancellationToken)
        {
            try
            {
                var invoices = await handler.GetAllInvoices();
                if (invoices.Data is null)
                {
                    await ResponseBuilder.BuildResponse<List<InvoiceResponse>>(invoices.Data, invoices.Success, invoices.Message,
                    HttpContext, StatusCodes.Status200OK, cancellationToken);
                }
                await ResponseBuilder.BuildResponse<List<InvoiceResponse>>(invoices.Data, invoices.Success, invoices.Message,
                    HttpContext, StatusCodes.Status200OK, cancellationToken);
            } catch (Exception ex)
            {
                await ResponseBuilder.BuildResponse<List<InvoiceResponse>>(null, false,
                    "Ha ocurrido un error durante el proceso, intente nuevamente más tarde: " + ex.Message,
                    HttpContext, StatusCodes.Status500InternalServerError, cancellationToken);
            }
        }
    }
}
