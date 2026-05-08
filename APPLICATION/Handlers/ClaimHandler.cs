using APPLICATION.Use_cases.Claims_case;
using APPLICATION.Use_cases.PaymentRecords_case;
using DOMAIN.Entities;
using DOMAIN.Interfaces;

namespace APPLICATION.Handlers
{
    public class ClaimHandler(IClaimRepository service, IInvoiceRepository invoiceService, IInvoiceHistoryRepository historyService)
    {
        public async Task<Response<List<ClaimResponse>>> GetAllClaims()
        {
            try
            {
                var claims = await service.GetAllClaims();
                if (claims.Count is 0)
                {
                    return new Response<List<ClaimResponse>>()
                    {
                        Data = [],
                        Success = false,
                        Message = "El sistema no posee registros de reclamos"
                    };
                }
                var success = claims.Select(c => new ClaimResponse()
                {
                    Claim_id = c.Claim_id,
                    Record_date = c.Record_date,
                    User = c.User_id,
                    Description = c.Description,
                    Invoice = c.Invoice_id,
                    Claim_amount = c.Claim_amount

                }).ToList();
                return new Response<List<ClaimResponse>>()
                {
                    Data = success,
                    Success = true,
                    Message = "Reclamo obtenido exitosamente"
                };
            }
            catch (Exception ex)
            {
                return new Response<List<ClaimResponse>>()
                {
                    Data = null,
                    Success = false,
                    Message = "Ha ocurrido un error al procesar la solicitud: " + ex.Message
                };
            }
        }
        public async Task<Response<ClaimResponse>> GetClaimById(int claim_id)
        {
            try
            {
                var found = await service.GetClaimById(claim_id);
                if (found is null)
                {
                    return new Response<ClaimResponse>()
                    {
                        Data = null,
                        Success = false,
                        Message = "No existe el reclamo con el identificador recibido"
                    };
                }

                var success = new ClaimResponse()
                {
                    Claim_id = found.Claim_id,
                    Record_date = found.Record_date,
                    User = found.User_id,
                    Description = found.Description,
                    Invoice = found.Invoice_id,
                    Claim_amount = found.Claim_amount
                };

                return new Response<ClaimResponse>()
                {
                    Data = success,
                    Success = true,
                    Message = "Registro de reclamo encontrado"
                };
            }
            catch (Exception ex)
            {
                return new Response<ClaimResponse>()
                {
                    Data = null,
                    Success = false,
                    Message = "Ha ocurrido un error al procesar la solicitud: " + ex.Message
                };
            }
        }

        public async Task<Response<ClaimResponse>> CreateClaim(ClaimRequest request)
        {
            try
            {
                // a) Obtener la factura y validar que exista
                var invoice = await invoiceService.GetInvoiceById(request.Invoice_id);
                if (invoice is null)
                    return new Response<ClaimResponse>
                    {
                        Data = null,
                        Success = false,
                        Message = "No se encontró la factura asociada al reclamo."
                    };

                // b) Validar que la factura no tenga reclamo activo
                if (invoice.Status == "Con reclamo")
                    return new Response<ClaimResponse>
                    {
                        Data = null,
                        Success = false,
                        Message = "No se puede registrar el reclamo: la factura tiene un reclamo activo."
                    };
                // d) Construir el reclamo  
                var new_Claim = new Claim()
                {
                    Record_date = request.Record_date,
                    User_id = request.User_id,
                    Description = request.Description,
                    Invoice_id = request.Invoice_id,
                    Claim_amount = request.Claim_amount
                };
                // e) Insertar el reclamo
                var created = await service.CreateClaim(new_Claim);
                if (created is null)
                    return new Response<ClaimResponse>
                    {
                        Data = null,
                        Success = false,
                        Message = "Ha habido un error al registrar el reclamo."
                    };

                // h) Actualizar status de la factura
                var current_status = invoice.Status;
                invoice.Status = "Con reclamo";
                await invoiceService.UpdateInvoice(invoice);

                // i) Registrar en InvoiceHistories
                var history = new InvoiceHistory
                {
                    Invoice_id = invoice.Invoice_id,
                    Record_date = DateTime.Now,
                    Action = "Reclamo registrado",
                    Description = $"Reclamo registrado por {request.User_id}, " +
                                         $"Motivo: {request.Description}, " +
                                            (request.Claim_amount is not 0 ? $"Monto reclamado: {request.Claim_amount}" : ""),
                    User_id = invoice.User_creator_id,
                    Previous_status = current_status,
                    New_status = "Con reclamo",
                    Paid_amount = null,
                    Pending_balance = invoice.Pending_balance,
                    Claim_id = created.Claim_id
                };
                await historyService.CreateHistory(history);

                var success = new ClaimResponse()
                {
                    Claim_id = created.Claim_id,
                    Record_date = created.Record_date,
                    User = created.User_id,
                    Description = created.Description,
                    Invoice = created.Invoice_id,
                    Claim_amount = created.Claim_amount
                };
                return new Response<ClaimResponse>()
                {
                    Data = success,
                    Success = true,
                    Message = "Reclamo creado exitosamente"
                };
            }
            catch (Exception ex)
            {
                return new Response<ClaimResponse>()
                {
                    Data = null,
                    Success = false,
                    Message = "Ha ocurrido un error al procesar la solicitud: " + ex.Message
                };
            }
        }
    }
}
