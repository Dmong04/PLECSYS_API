using APPLICATION.Use_cases.InvoiceHistories_case;
using DOMAIN.Interfaces;
namespace APPLICATION.Handlers
{
    public class InvoiceHistoryHandler(IInvoiceHistoryRepository service, IPaymentRecordRepository paymentService, IClaimRepository claimService, IInvoiceRepository invoiceService)
    {
        public async Task<Response<List<InvoiceHistoryResponse>>> GetAllInvoiceHistoriesByUserAndCompanyId(FindHistoriesRequest request)
        {
            try
            {
                var invoice_histories = await service.GetAllInvoiceHistoriesByUserAndCompanyId(request.Email, request.CompanyId);
                if (invoice_histories.Count is 0)
                {
                    return new Response<List<InvoiceHistoryResponse>>()
                    {
                        Data = [],
                        Success = false,
                        Message = "El sistema no posee registros de historico"
                    };
                }
                var success = invoice_histories.Select(h => new InvoiceHistoryResponse()
                {
                    Invoice_history_id = h.Invoice_history_id,
                    Invoice_id = h.Invoice_id,
                    Record_date = h.Record_date,
                    Action = h.Action,
                    Description = h.Description,
                    User_id = h.User_id,
                    Previous_status = h.Previous_status,
                    New_status = h.New_status,
                    Paid_amount = h.Paid_amount,
                    Pending_balance = h.Pending_balance,
                    Payment_record_id = h.Payment_record_id,
                    Claim_id = h.Claim_id
                }).ToList();
                return new Response<List<InvoiceHistoryResponse>>()
                {
                    Data = success,
                    Success = true,
                    Message = "Historial de facturas obtenido exitosamente"
                };
            }
            catch (Exception ex)
            {
                return new Response<List<InvoiceHistoryResponse>>()
                {
                    Data = null,
                    Success = false,
                    Message = "Ha ocurrido un error al procesar la solicitud: " + ex.Message
                };
            }
        }

        public async Task<Response<InvoiceHistoryResponse>> GetInvoiceHistoryById(int invoice_history_id)
        {
            try
            {
                var found = await service.GetInvoiceHistoriesById(invoice_history_id);
                if (found is null)
                {
                    return new Response<InvoiceHistoryResponse>()
                    {
                        Data = null,
                        Success = false,
                        Message = "No existe el historial de facturas con el identificador recibido"
                    };
                }

                var success = new InvoiceHistoryResponse()
                {
                    Invoice_history_id = invoice_history_id,
                    Invoice_id = found.Invoice_id,
                    Record_date = found.Record_date,
                    Action = found.Action,
                    Description = found.Description,
                    User_id = found.User_id,
                    Previous_status = found.Previous_status,
                    New_status = found.New_status,
                    Paid_amount = found.Paid_amount,
                    Pending_balance = found.Pending_balance,
                    Payment_record_id = found.Payment_record_id,
                    Claim_id = found.Claim_id

                };

                return new Response<InvoiceHistoryResponse>()
                {
                    Data = success,
                    Success = true,
                    Message = "Historial de facturas encontrado"
                };
            }
            catch (Exception ex)
            {
                return new Response<InvoiceHistoryResponse>()
                {
                    Data = null,
                    Success = false,
                    Message = "Ha ocurrido un error al procesar la solicitud: " + ex.Message
                };
            }
        }

        public async Task<Response<List<InvoiceHistoryResponse>>> GetInvoiceHistoriesByInvoiceId(int invoice_id)
        {
            try
            {
                var histories = await service.GetHistoriesByInvoiceId(invoice_id);

                if (histories.Count == 0)
                {
                    return new Response<List<InvoiceHistoryResponse>>()
                    {
                        Data = [],
                        Success = false,
                        Message = "La factura no posee registros de historial"
                    };
                }

                var success = histories.Select(h => new InvoiceHistoryResponse()
                {
                    Invoice_history_id = h.Invoice_history_id,
                    Invoice_id = h.Invoice_id,
                    Record_date = h.Record_date,
                    Action = h.Action,
                    Description = h.Description,
                    User_id = h.User_id,
                    Previous_status = h.Previous_status,
                    New_status = h.New_status,
                    Paid_amount = h.Paid_amount,
                    Pending_balance = h.Pending_balance,
                    Payment_record_id = h.Payment_record_id,
                    Claim_id = h.Claim_id
                }).ToList();

                return new Response<List<InvoiceHistoryResponse>>()
                {
                    Data = success,
                    Success = true,
                    Message = "Historial de la factura obtenido correctamente"
                };
            }
            catch (Exception ex)
            {
                return new Response<List<InvoiceHistoryResponse>>()
                {
                    Data = null,
                    Success = false,
                    Message = "Ha ocurrido un error al procesar la solicitud: " + ex.Message
                };
            }
        }

        public async Task<Response<List<InvoiceHistoryResponse>>> GetInvoicePaymentHistories(int invoice_id)
        {
            try
            {
                var invoice = await invoiceService.GetInvoiceById(invoice_id);
                if (invoice is null)
                {
                    return new Response<List<InvoiceHistoryResponse>>()
                    {
                        Data = [],
                        Success = false,
                        Message = "No se encontró la factura solicitada."
                    };
                }

                // Obtener registros de pago asociados a la factura
                var payments = (await paymentService.GetAllPaymentRecords())
                                .Where(p => p.Source_id == invoice_id)
                                .ToList();

                if (payments.Count == 0)
                {
                    return new Response<List<InvoiceHistoryResponse>>()
                    {
                        Data = [],
                        Success = false,
                        Message = "La factura no posee registros de pago."
                    };
                }

                var success = payments.Select(p => new InvoiceHistoryResponse()
                {
                    Invoice_history_id = null,
                    Invoice_id = p.Source_id,
                    Record_date = p.Payment_date,
                    Action = "Pago registrado",
                    Description = p.Payment_detail,
                    User_id = invoice.User_creator_id,
                    Previous_status = null,
                    New_status = invoice.Status,
                    Paid_amount = p.Paid_amount,
                    Pending_balance = invoice.Pending_balance,
                    Payment_record_id = p.Payment_record_id,
                    Claim_id = null
                }).ToList();

                return new Response<List<InvoiceHistoryResponse>>()
                {
                    Data = success,
                    Success = true,
                    Message = "Pagos de la factura obtenidos correctamente"
                };
            }
            catch (Exception ex)
            {
                return new Response<List<InvoiceHistoryResponse>>()
                {
                    Data = null,
                    Success = false,
                    Message = "Ha ocurrido un error al procesar la solicitud: " + ex.Message
                };
            }
        }

        public async Task<Response<List<InvoiceHistoryResponse>>> GetInvoiceClaimsHistories(int invoice_id)
        {
            try
            {
                // Obtener la factura y validar existencia
                var invoice = await invoiceService.GetInvoiceById(invoice_id);
                if (invoice is null)
                {
                    return new Response<List<InvoiceHistoryResponse>>()
                    {
                        Data = [],
                        Success = false,
                        Message = "No se encontró la factura solicitada."
                    };
                }

                // Verificar si la factura tiene el estado que indica reclamo
                if (!string.Equals(invoice.Status, "Con reclamo", StringComparison.OrdinalIgnoreCase))
                {
                    return new Response<List<InvoiceHistoryResponse>>()
                    {
                        Data = [],
                        Success = false,
                        Message = $"La factura no tiene reclamos. Estado actual: '{invoice.Status ?? "Sin estado"}'."
                    };
                }

                // Obtener reclamos asociados a la factura
                var claims = (await claimService.GetAllClaims())
                                .Where(c => c.Invoice_id == invoice_id)
                                .ToList();

                if (claims.Count == 0)
                {
                    return new Response<List<InvoiceHistoryResponse>>()
                    {
                        Data = [],
                        Success = false,
                        Message = "No se encontraron reclamos asociados a la factura."
                    };
                }

                // Mapear reclamos a InvoiceHistoryResponse para la UI
                var success = claims.Select(c => new InvoiceHistoryResponse()
                {
                    Invoice_history_id = null,
                    Invoice_id = c.Invoice_id,
                    Record_date = c.Record_date,
                    Action = "Reclamo registrado",
                    Description = c.Description,
                    User_id = c.User_id,
                    Previous_status = null,
                    New_status = invoice.Status,
                    Paid_amount = null,
                    Pending_balance = invoice?.Pending_balance,
                    Payment_record_id = null,
                    Claim_id = c.Claim_id
                }).ToList();

                return new Response<List<InvoiceHistoryResponse>>()
                {
                    Data = success,
                    Success = true,
                    Message = "Reclamos de la factura obtenidos correctamente"
                };
            }
            catch (Exception ex)
            {
                return new Response<List<InvoiceHistoryResponse>>()
                {
                    Data = null,
                    Success = false,
                    Message = "Ha ocurrido un error al procesar la solicitud: " + ex.Message
                };
            }
        }
    }
}
