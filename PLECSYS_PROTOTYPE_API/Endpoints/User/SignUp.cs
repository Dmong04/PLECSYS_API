using APPLICATION.Handlers;
using APPLICATION.Use_cases.Login_case;
using APPLICATION.Use_cases.SignUp_case;
using FastEndpoints;
using PLECSYS_PROTOTYPE_API.API_response;


namespace PLECSYS_PROTOTYPE_API.Endpoints.User
{
    public class SignUp(UserHandler _handler) : Endpoint<SignUpRequest, SignUpResponse>
    {
        public override void Configure()
        {
            Post("api/v1/plecsys/user/signup");
            AllowAnonymous();
        }

        public override async Task HandleAsync(SignUpRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var signed = await _handler.SignUpHandler(request);
                if (signed.Data is null)
                    await ResponseBuilder.BuildResponse<SignUpResponse>(signed.Data, signed.Success, signed.Message,
                    HttpContext, StatusCodes.Status400BadRequest, cancellationToken);
                await ResponseBuilder.BuildResponse<SignUpResponse>(signed.Data, signed.Success, signed.Message,
                    HttpContext, StatusCodes.Status200OK, cancellationToken);
            }
            catch (Exception ex)
            {
                await ResponseBuilder.BuildResponse<SignUpResponse>(null, false,
                    "Ha ocurrido un error durante el proceso, intente nuevamente más tarde: " + ex.Message,
                    HttpContext, StatusCodes.Status500InternalServerError, cancellationToken);
            }
        }
    }
}
