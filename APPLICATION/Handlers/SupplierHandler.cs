using APPLICATION.Use_cases.Suppliers_case;
using DOMAIN.Interfaces;

namespace APPLICATION.Handlers
{
    public class SupplierHandler(ISupplierRepository _repo, ICompanyRepository _company)
    {
        public async Task<Response<List<SupplierResponse>>> GetSuppliersByCompanyId(int companyId)
        {
            try
            {
                var found = await _company.GetCompanyById(companyId);
                if (found is null)
                {
                    return new Response<List<SupplierResponse>>()
                    {
                        Data = [],
                        Success = false,
                        Message = "No existe la compañía seleccionada"
                    };
                }

                var request = await _repo.GetSuppliersByCompanyId(found.Company_id);
                if (request.Count is 0)
                {
                    return new Response<List<SupplierResponse>>()
                    {
                        Data = [],
                        Success = true,
                        Message = "No hay clientes asociados a esta compañía"
                    };
                }

                var success = request.Select(s => new SupplierResponse()
                {
                    Supplier_id = s.Supplier_id,
                    Supplier_name = s.Supplier_name,
                }).ToList();

                return new Response<List<SupplierResponse>>()
                {
                    Data = success,
                    Success = true,
                    Message = "Clientes asociados con éxito"
                };
            }
            catch (Exception ex)
            {
                return new Response<List<SupplierResponse>>()
                {
                    Data = [],
                    Success = false,
                    Message = $"Error al procesar la solicitud: {ex.Message}"
                };
            }
        }
    }
}
