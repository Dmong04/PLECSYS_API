using DOMAIN.Entities.PLECSYS_Records.Items;

namespace APPLICATION.Use_cases.Items_case
{
    public interface IItemUseCase
    {
        Task<ItemApiResponse> ExecuteAsync(ItemsRequest request, string token);
    }
}
