using APPLICATION.Handlers;
using APPLICATION.Use_cases.PaymentMethods_case;
using APPLICATION.Use_cases.PaymentRecords_case;
using DOMAIN.Entities;
using FastEndpoints;
using PLECSYS_PROTOTYPE_API.API_response;

namespace PLECSYS_PROTOTYPE_API.Endpoints.PaymentRecords
{
    public class GetAllPaymentRecords(PaymentRecordHandler _handler) : EndpointWithoutRequest<List<PaymentMethodResponse>>
    {
        public override void Configure()
        {
            Get("api/v1/plecsys/payment/record/all");
            AllowAnonymous();
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            try
            {
                var payment_records = await _handler.GetAllPaymentRecords();
                if (payment_records.Data.Count is 0)
                    await ResponseBuilder.BuildResponse<List<PaymentRecordResponse>>(payment_records.Data, payment_records.Success,
                        payment_records.Message, HttpContext, StatusCodes.Status200OK, ct);
                await ResponseBuilder.BuildResponse<List<PaymentRecordResponse>>(payment_records.Data, payment_records.Success,
                        payment_records.Message, HttpContext, StatusCodes.Status200OK, ct);
            } catch (Exception ex)
            {
                await ResponseBuilder.BuildResponse<List<PaymentRecordResponse>>(null, false,
                        "Hubo un error al procesar la solicitud: " + ex.Message, 
                        HttpContext, StatusCodes.Status500InternalServerError, ct);
            }
        }
    }
}
