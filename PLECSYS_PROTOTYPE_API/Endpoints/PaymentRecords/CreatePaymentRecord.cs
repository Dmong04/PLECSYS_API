using APPLICATION.Handlers;
using APPLICATION.Use_cases.PaymentMethods_case;
using APPLICATION.Use_cases.PaymentRecords_case;
using FastEndpoints;
using PLECSYS_PROTOTYPE_API.API_response;

namespace PLECSYS_PROTOTYPE_API.Endpoints.PaymentRecords
{
    public class CreatePaymentRecord(PaymentRecordHandler _handler) : Endpoint<PaymentRecordRequest, PaymentRecordResponse>
    {
        public override void Configure()
        {
            Post("api/v1/plecsys/payment/record/create");
            AllowAnonymous();
        }

        public override async Task HandleAsync(PaymentRecordRequest request, CancellationToken ct)
        {
            try
            {
                var created = await _handler.CreatePaymentRecord(request);
                if (created.Data is null)
                {
                    await ResponseBuilder.BuildResponse<PaymentRecordResponse>(created.Data, created.Success, created.Message,
                        HttpContext, StatusCodes.Status400BadRequest, ct);
                    return;
                }
                await ResponseBuilder.BuildResponse<PaymentRecordResponse>(created.Data, created.Success, created.Message,
                        HttpContext, StatusCodes.Status200OK, ct);
            }
            catch (Exception ex)
            {
                await ResponseBuilder.BuildResponse<PaymentRecordResponse>(null, false,
                    "Ha ocurrido un error en el proceso: " + ex.Message, HttpContext, StatusCodes.Status500InternalServerError, ct);
            }
        }
    }
}
