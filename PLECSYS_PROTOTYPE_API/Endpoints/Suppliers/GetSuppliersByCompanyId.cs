using APPLICATION.Handlers;
using APPLICATION.Use_cases.Suppliers_case;
using FastEndpoints;
using PLECSYS_PROTOTYPE_API.API_response;

namespace PLECSYS_PROTOTYPE_API.Endpoints.Suppliers
{
    public class GetSuppliersByCompanyId(SupplierHandler _handler) : EndpointWithoutRequest<List<SupplierResponse>>
    {
        public override void Configure()
        {
            Get("api/v1/plecsys/supplier/{company_id}");
            AllowAnonymous();
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            try
            {
                int companyId = Route<int>("company_id");
                var response = await _handler.GetSuppliersByCompanyId(companyId);
                await ResponseBuilder.BuildResponse<List<SupplierResponse>>(response.Data, response.Success,
                    response.Message, HttpContext, StatusCodes.Status200OK, ct);
            } catch (Exception ex)
            {
                await ResponseBuilder.BuildResponse<List<SupplierResponse>>(null, false,
                        $"Hubo un error al procesar la solicitud: {ex.Message}",
                        HttpContext, StatusCodes.Status500InternalServerError, ct);
            }
        }
    }
}
