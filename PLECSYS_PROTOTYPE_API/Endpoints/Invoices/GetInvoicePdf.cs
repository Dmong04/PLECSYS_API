using APPLICATION.Handlers;
using FastEndpoints;
using PLECSYS_PROTOTYPE_API.API_response;

namespace PLECSYS_PROTOTYPE_API.Endpoints.Invoices
{
    public class GetInvoicePdf(InvoiceHandler handler) : EndpointWithoutRequest
    {
        public override void Configure()
        {
            Get("api/v1/plecsys/invoice/{invoiceId}/pdf");
            AllowAnonymous();
        }

        public override async Task HandleAsync(CancellationToken cancellationToken)
        {
            try
            {
                var invoiceId = Route<int>("invoiceId");
                var result = await handler.GetInvoicePdf(invoiceId, cancellationToken);

                if (!result.Success || result.Data is null)
                {
                    await ResponseBuilder.BuildResponse<byte[]>(null, false, result.Message,
                        HttpContext, StatusCodes.Status404NotFound, cancellationToken);
                    return;
                }

                HttpContext.Response.ContentType = "application/pdf";
                HttpContext.Response.Headers.ContentDisposition = $"attachment; filename=\"Factura_{invoiceId:D6}.pdf\"";
                await HttpContext.Response.Body.WriteAsync(result.Data, cancellationToken);
            }
            catch (Exception ex)
            {
                await ResponseBuilder.BuildResponse<byte[]>(null, false,
                    "Ha ocurrido un error durante el proceso, intente nuevamente más tarde: " + ex.Message,
                    HttpContext, StatusCodes.Status500InternalServerError, cancellationToken);
            }
        }
    }
}