using DOMAIN.Entities.PLECSYS_Records.Items;
using DOMAIN.Interfaces;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace INFRASTRUCTURE.Repositories
{
    public class ItemRepository : IItemRepository
    {
        private readonly HttpClient _http;

        public ItemRepository(IHttpClientFactory http)
        {
            _http = http.CreateClient("AuthServer");
            _http.DefaultRequestHeaders.Accept.Clear();
            _http.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
        public async Task<ItemApiResponse> GetItemsAsync(ItemsRequest request, string token)
        {
            var req = JsonSerializer.Serialize(request);

            var httpRequest = new HttpRequestMessage(HttpMethod.Post, "api/Invoice/GetItemsFromApp");
            httpRequest.Content = new StringContent(req, Encoding.UTF8, "application/json");
            httpRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _http.SendAsync(httpRequest);
            var raw = await response.Content.ReadAsStringAsync();

            response.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<ItemApiResponse>(raw, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }) ?? new ItemApiResponse
            {
                Success = false,
                Message = "Failed to deserialize response."
            };
        }
    }
}
