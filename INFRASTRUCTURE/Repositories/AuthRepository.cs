using DOMAIN;
using DOMAIN.Entities.PLECSYS_Records;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace INFRASTRUCTURE.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly HttpClient _http;
        private readonly string _apiKey;

        public AuthRepository(IHttpClientFactory factory, IConfiguration configuration)
        {
            _http = factory.CreateClient("AuthServer");
            _http.DefaultRequestHeaders.Accept.Clear();
            _http.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            _apiKey = configuration["AuthServer:Apikey"]
                ?? throw new InvalidOperationException("Falta configurar AuthServer:Apikey");
        }

        public async Task<TokenResponse> LoginAsync(OAuthLoginRequest request, CancellationToken ct = default)
        {
            var payload = JsonSerializer.Serialize(new
            {
                grant_type = request.GrantType,
                request.Email,
                request.Password
            });

            using var httpRequest = new HttpRequestMessage(HttpMethod.Post, "/api/Account/Login");
            httpRequest.Content = new StringContent(payload, Encoding.UTF8, "application/json");
            httpRequest.Headers.Add("X-API-KEY", _apiKey);

            using var response = await _http.SendAsync(httpRequest, ct);
            var raw = await response.Content.ReadAsStringAsync(ct);

            if (!response.IsSuccessStatusCode)
                throw new HttpRequestException($"Auth login failed ({(int)response.StatusCode}). Body: {raw}");

            var token = JsonSerializer.Deserialize<TokenResponse>(
                raw,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
            );

            if (token == null || string.IsNullOrWhiteSpace(token.AccessToken))
                throw new Exception("Auth server devolvió una respuesta inválida (sin access_token).");

            return token;
        }
    }
}