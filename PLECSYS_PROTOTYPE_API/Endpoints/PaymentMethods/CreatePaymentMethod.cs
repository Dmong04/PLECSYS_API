using APPLICATION.Handlers;
using APPLICATION.Use_cases.PaymentMethods_case;
using FastEndpoints;
using PLECSYS_PROTOTYPE_API.API_response;

namespace PLECSYS_PROTOTYPE_API.Endpoints.PaymentMethod
{
    public class CreatePaymentMethod(PaymentMethodHandler _handler) : Endpoint<PaymentMethodRequest, PaymentMethodResponse>
    {
        public override void Configure()
        {
            Post("api/v1/plecsys/payment/method/create");
            AllowAnonymous();
        }

        public override async Task HandleAsync(PaymentMethodRequest request, CancellationToken ct)
        {
            try
            {
                var created = await _handler.CreatePaymentMethod(request);
                if (!created.Success)
                {
                    await ResponseBuilder.BuildResponse<PaymentMethodResponse>(created.Data, false,
                    "No se ha creado el método de pago porque este ya existe", 
                    HttpContext, StatusCodes.Status400BadRequest, ct);
                }
                await ResponseBuilder.BuildResponse<PaymentMethodResponse>(created.Data, true,
                    "Método de pago creado con éxito",
                    HttpContext, StatusCodes.Status200OK, ct);
            } catch (Exception ex)
            {
                await ResponseBuilder.BuildResponse<PaymentMethodResponse>(null, false,
                    "Ha ocurrido un error en el proceso: " + ex.Message, HttpContext, StatusCodes.Status500InternalServerError, ct);
            }
        }
    }
}
