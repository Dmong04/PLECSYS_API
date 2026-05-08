using DOMAIN;
using DOMAIN.Entities.PLECSYS_Records;

namespace APPLICATION.Use_cases.Login_case
{
    public class LoginUseCase : ILoginUseCase
    {
        private readonly IAuthRepository _authRepository;

        public LoginUseCase(IAuthRepository authRepository)
        {
            _authRepository = authRepository;
        }

        public async Task<TokenResponse> ExecuteAsync(LoginRequest request, CancellationToken ct = default)
        {
            var oauthRequest = new OAuthLoginRequest
            {
                GrantType = "password",
                Email = request.Email,
                Password = request.Password
            };

            return await _authRepository.LoginAsync(oauthRequest, ct);
        }

    }
}
