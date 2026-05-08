using DOMAIN.Entities.PLECSYS_Records.Customers;
using DOMAIN.Interfaces;

namespace APPLICATION.Use_cases.Customers_case
{
    public class CustomerUseCase(ICustomerRepository _repo) : ICustomerUseCase
    {
        public async Task<CustomerApiResponse> ExecuteAsync(CustomersRequest request, string token)
        {
            var req = new CustomersRequest
            {
                Company_id = request.Company_id,
                User_id = request.User_id
            };

            return await _repo.GetCustomersAsync(req, token);
        }
    }
}
