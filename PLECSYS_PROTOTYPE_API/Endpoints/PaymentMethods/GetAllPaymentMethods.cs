using APPLICATION.Handlers;
using APPLICATION.Use_cases.PaymentMethods_case;
using FastEndpoints;
using PLECSYS_PROTOTYPE_API.API_response;

namespace PLECSYS_PROTOTYPE_API.Endpoints.PaymentMethod
{
    public class GetAllPaymentMethods(PaymentMethodHandler _handler) : EndpointWithoutRequest<List<PaymentMethodResponse>>
    {
        public override void Configure()
        {
            Get("api/v1/plecsys/payment/method/all");
            AllowAnonymous();
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            try
            {
                var paymentMethods = await _handler.GetAllPaymentMethods();
                if (paymentMethods.Data is null)
                {
                    await ResponseBuilder.BuildResponse<List<PaymentMethodResponse>>(null, false,
                    "No hay métodos de pago registrados en el servidor", HttpContext, StatusCodes.Status200OK, ct);
                }
                await ResponseBuilder.BuildResponse<List<PaymentMethodResponse>>(paymentMethods.Data, true,
                    "Se obtuvo el listado de métodos de pago exitosamente", HttpContext, StatusCodes.Status200OK, ct);
            } catch (Exception ex)
            {
                await ResponseBuilder.BuildResponse<List<PaymentMethodResponse>>(null, false,
                    "Ha ocurrido un error en el proceso: " + ex.Message, HttpContext, StatusCodes.Status500InternalServerError, ct);
            }
        }
    }
}
