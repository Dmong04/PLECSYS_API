using APPLICATION.Use_cases.Customers_case;
using DOMAIN.Entities.PLECSYS_Records.Customers;
using FastEndpoints;
using PLECSYS_PROTOTYPE_API.API_response;

namespace PLECSYS_PROTOTYPE_API.Endpoints.Customers
{
    public class GetCustomers(ICustomerUseCase _useCase) : Endpoint<CustomersRequest, CustomerApiResponse>
    {
        public override void Configure()
        {
            Post("api/Invoice/GetCustomersFromApp");
            AllowAnonymous();
        }

        public override async Task HandleAsync(CustomersRequest req, CancellationToken ct)
        {
            var token = HttpContext.Request.Headers.Authorization.ToString().Replace("Bearer ", "");
            var response = await _useCase.ExecuteAsync(req, token);

            await ResponseBuilder.BuildResponse(response.Data, response.Success, response.Message, 
                HttpContext, StatusCodes.Status200OK, ct);
        }
    }
}
