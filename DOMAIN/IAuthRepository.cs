using DOMAIN.Entities.PLECSYS_Records;
using System;
namespace DOMAIN
{
    public interface IAuthRepository
    {
        Task<TokenResponse> LoginAsync(OAuthLoginRequest request, CancellationToken ct = default);
    }
}
