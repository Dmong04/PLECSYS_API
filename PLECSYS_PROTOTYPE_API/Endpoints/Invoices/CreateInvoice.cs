using APPLICATION.Handlers;
using APPLICATION.Use_cases.Companies_case;
using APPLICATION.Use_cases.Invoices_case;
using FastEndpoints;
using PLECSYS_PROTOTYPE_API.API_response;

namespace PLECSYS_PROTOTYPE_API.Endpoints.Invoices
{
    public class CreateInvoice(InvoiceHandler _handler) : Endpoint<InvoiceRequest, InvoiceResponse>
    {
        public override void Configure()
        {
            Post("api/v1/plecsys/invoice/create");
            AllowAnonymous();
        }

        public override async Task HandleAsync(InvoiceRequest request, CancellationToken ct)
        {
            try
            {
                var created = await _handler.CreateInvoice(request);
                if (created.Data is null)
                {
                    await ResponseBuilder.BuildResponse<InvoiceResponse>(null, false,
                        "No se pudo guardar la factura", HttpContext, StatusCodes.Status400BadRequest, ct);
                }
                await ResponseBuilder.BuildResponse<InvoiceResponse>(created.Data, true,
                        "Se ha guardado con éxito la factura", HttpContext, StatusCodes.Status200OK, ct);
            } catch (Exception ex)
            {
                await ResponseBuilder.BuildResponse<InvoiceResponse>(null, false,
                        "Ha ocurrido un error en el proceso:" + ex.Message, HttpContext, StatusCodes.Status500InternalServerError, ct);
            }
        }
    }
}
