using APPLICATION.Use_cases.Login_case;
using DOMAIN.Entities.PLECSYS_Records;
using FastEndpoints;
using PLECSYS_PROTOTYPE_API.API_response;

namespace PLECSYS_PROTOTYPE_API.Endpoints.Auth
{
    public class LoginEndpoint : Endpoint<LoginRequest, TokenResponse>
    {
        private readonly ILoginUseCase _loginUseCase;

        public LoginEndpoint(ILoginUseCase loginUseCase)
        {
            _loginUseCase = loginUseCase;
        }

        public override void Configure()
        {
            Post("/api/Account/Login");
            AllowAnonymous();
        }

        public override async Task HandleAsync(LoginRequest req, CancellationToken ct)
        {
            try
            {
                var token = await _loginUseCase.ExecuteAsync(req, ct);
                await ResponseBuilder.BuildResponse<TokenResponse>(
                    token, true, "Login exitoso",
                    HttpContext, StatusCodes.Status200OK, ct);
            }
            catch (HttpRequestException ex) when (ex.Message.Contains("401"))
            {
                await ResponseBuilder.BuildResponse<TokenResponse>(
                    null, false, "Credenciales inválidas.",
                    HttpContext, StatusCodes.Status401Unauthorized, ct);
            }
            catch (Exception ex)
            {
                await ResponseBuilder.BuildResponse<TokenResponse>(
                    null, false, "Hubo un error al procesar la solicitud: " + ex.Message,
                    HttpContext, StatusCodes.Status500InternalServerError, ct);
            }
        }
    }
}