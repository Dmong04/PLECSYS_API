using DOMAIN.Entities.PLECSYS_Records.Items;
using DOMAIN.Interfaces;

namespace APPLICATION.Use_cases.Items_case
{
    public class ItemUseCase(IItemRepository _repo) : IItemUseCase
    {
        public async Task<ItemApiResponse> ExecuteAsync(ItemsRequest request, string token)
        {
            return await _repo.GetItemsAsync(request, token);
        }
    }
}
