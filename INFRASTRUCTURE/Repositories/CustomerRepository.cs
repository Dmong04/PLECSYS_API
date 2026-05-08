using DOMAIN.Entities.PLECSYS_Records.Customers;
using DOMAIN.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace INFRASTRUCTURE.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {

        private readonly HttpClient _http;

        public CustomerRepository(IHttpClientFactory http, IConfiguration config)
        {
            _http = http.CreateClient("AuthServer");
            _http.DefaultRequestHeaders.Accept.Clear();
            _http.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
        public async Task<CustomerApiResponse> GetCustomersAsync(CustomersRequest request, string token)
        {
            var req = JsonSerializer.Serialize(request);

            using var httpRequest = new HttpRequestMessage(HttpMethod.Post, "api/Invoice/GetCustomersFromApp");
            httpRequest.Content = new StringContent(req, Encoding.UTF8, "application/json");
            httpRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            using var response = await _http.SendAsync(httpRequest);
            var raw = await response.Content.ReadAsStringAsync();

            response.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<CustomerApiResponse>(raw, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }) ?? new CustomerApiResponse { Success = false, Message = "Failed to deserialize response." };
        }
    }
}