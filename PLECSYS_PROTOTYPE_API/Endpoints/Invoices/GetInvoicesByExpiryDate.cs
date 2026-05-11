using APPLICATION.Handlers;
using APPLICATION.Use_cases.Invoices_case;
using FastEndpoints;
using PLECSYS_PROTOTYPE_API.API_response;

namespace PLECSYS_PROTOTYPE_API.Endpoints.Invoices
{
    public class GetInvoicesByExpiryDate(InvoiceHandler _handler)
        : Endpoint<GetInvoicesByExpiryDateRequest, List<InvoiceResponse>>
    {
        public override void Configure()
        {
            Post("api/v1/plecsys/invoice/expiry");
            AllowAnonymous();
        }

        public override async Task HandleAsync(GetInvoicesByExpiryDateRequest req, CancellationToken ct)
        {
            try
            {
                var invoices = await _handler.GetInvoicesByExpiryDateAndUserEmail(
                    req.Email, req.CompanyId, req.ExpiryDate);

                if (invoices.Data is null)
                {
                    await ResponseBuilder.BuildResponse<List<InvoiceResponse>>(invoices.Data, invoices.Success, invoices.Message,
                        HttpContext, StatusCodes.Status400BadRequest, ct);
                    return;
                }

                await ResponseBuilder.BuildResponse<List<InvoiceResponse>>(invoices.Data, invoices.Success, invoices.Message,
                    HttpContext, StatusCodes.Status200OK, ct);
            }
            catch (Exception ex)
            {
                await ResponseBuilder.BuildResponse<List<InvoiceResponse>>(null, false,
                    "Hubo un error al procesar la solicitud: " + ex.Message,
                    HttpContext, StatusCodes.Status500InternalServerError, ct);
            }
        }
    }
}