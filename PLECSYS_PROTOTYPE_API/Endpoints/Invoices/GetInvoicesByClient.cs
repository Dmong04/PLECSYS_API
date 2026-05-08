using APPLICATION.Handlers;
using APPLICATION.Use_cases.Invoices_case;
using FastEndpoints;
using PLECSYS_PROTOTYPE_API.API_response;

namespace PLECSYS_PROTOTYPE_API.Endpoints.Invoices
{
    public class GetInvoicesByClient(InvoiceHandler _handler) : EndpointWithoutRequest<List<InvoiceResponse>>
    {
        public override void Configure()
        {
            Get("api/v1/plecsys/invoice/client/{email}");  
            AllowAnonymous();
        }
        public override async Task HandleAsync(CancellationToken ct)
        {
            try
            {
                var email = Route<string>("email");

                if (string.IsNullOrWhiteSpace(email))
                {
                    await ResponseBuilder.BuildResponse<List<InvoiceResponse>>(null, false,
                        "El correo del cliente es requerido.", HttpContext, StatusCodes.Status400BadRequest, ct);
                    return;
                }

                var invoices = await _handler.GetInvoicesByClient(email);
                if (invoices.Data is null || !invoices.Data.Any())
                {
                    await ResponseBuilder.BuildResponse<List<InvoiceResponse>>(null, false,
                        "No se encontraron facturas para el cliente con el correo proporcionado", HttpContext, StatusCodes.Status200OK, ct);
                    return;
                }
                await ResponseBuilder.BuildResponse<List<InvoiceResponse>>(invoices.Data, true,
                    "Se obtuvieron las facturas del cliente exitosamente", HttpContext, StatusCodes.Status200OK, ct);
            }
            catch (Exception ex)
            {
                await ResponseBuilder.BuildResponse<List<InvoiceResponse>>(null, false,
                    "Ha ocurrido un error en el proceso: " + ex.Message, HttpContext, StatusCodes.Status500InternalServerError, ct);
            }
        }
    }
}
