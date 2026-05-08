using APPLICATION.Handlers;
using APPLICATION.Use_cases.Login_case;
using FastEndpoints;
using PLECSYS_PROTOTYPE_API.API_response;

namespace PLECSYS_PROTOTYPE_API.Endpoints.User
{
    public class Login(UserHandler _handler) : Endpoint<LoginRequestDTO, LoginResponse>
    {
        public override void Configure()
        {
            Post("api/v1/plecsys/user/login");
            AllowAnonymous();
        }

        public override async Task HandleAsync(LoginRequestDTO request, CancellationToken ct)
        {
            try
            {
               /* var logged_in = await _handler.LoginHandler(request);
                // if (!logged_in.Data.Is_logged)
                    await ResponseBuilder.BuildResponse<LoginResponse>(logged_in.Data, logged_in.Success, logged_in.Message,
                        HttpContext, StatusCodes.Status400BadRequest, ct);
                await ResponseBuilder.BuildResponse<LoginResponse>(logged_in.Data, logged_in.Success, logged_in.Message,
                        HttpContext, StatusCodes.Status200OK, ct);*/
            }
            catch (Exception ex)
            {
                await ResponseBuilder.BuildResponse<LoginResponse>(null, false,
                    "Hubo un error al procesar la solicitud: " + ex.Message,
                    HttpContext, StatusCodes.Status500InternalServerError, ct);
            }
        }
    }
}
