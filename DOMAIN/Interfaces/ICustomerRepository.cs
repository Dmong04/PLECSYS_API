using DOMAIN.Entities.PLECSYS_Records.Customers;

namespace DOMAIN.Interfaces
{
    public interface ICustomerRepository
    {
        Task<CustomerApiResponse> GetCustomersAsync(CustomersRequest request, string token);
    }
}
