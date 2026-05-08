using DOMAIN.Entities.PLECSYS_Records.Customers;

namespace APPLICATION.Use_cases.Customers_case
{
    public interface ICustomerUseCase
    {
        Task<CustomerApiResponse> ExecuteAsync(CustomersRequest request, string token);
    }
}
