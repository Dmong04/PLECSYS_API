using DOMAIN.Entities.PLECSYS_Records.Items;

namespace DOMAIN.Interfaces
{
    public interface IItemRepository
    {
        Task<ItemApiResponse> GetItemsAsync(ItemsRequest request, string token);
    }
}
