using DOMAIN.Entities.PLECSYS_Records;

namespace APPLICATION.Use_cases.Login_case
{
    public interface ILoginUseCase
    {
        Task<TokenResponse> ExecuteAsync(LoginRequest request, CancellationToken ct = default);
    }
}
