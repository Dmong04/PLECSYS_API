using APPLICATION.Use_cases.Invoices_case;
using APPLICATION.Utils.PDFs;
using DOMAIN.Entities;
using DOMAIN.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APPLICATION.Handlers
{
    public class InvoiceHandler(IInvoiceRepository service, InvoicePdfService pdfService)
    {
        public async Task<Response<InvoiceResponse>> CreateInvoice(InvoiceRequest request)
        {
            try
            {
                var new_invoice = new Invoice()
                {
                    Consecutive = request.Consecutive,
                    Total_voucher = request.Total_voucher,
                    User_creator_id = request.User_creator_id,
                    Sell_company_id = request.Sell_company_id,
                    Charged_company_id = request.Charged_company_id,
                    Currency_id = request.Currency_id
                };

                var created = await service.CreateInvoice(new_invoice);
                if (created is null)
                {
                    return new Response<InvoiceResponse>()
                    {
                        Data = null,
                        Success = false,
                        Message = "No se pudo agregar la factura al sistema"
                    };
                }

                var success = new InvoiceResponse()
                {
                    Invoice_id = created.Invoice_id,
                    Consecutive = created.Consecutive,
                    Total_voucher = created.Total_voucher,
                    User = new User()
                    {
                        Email = created.User.Email,
                        Name = created.User.Name,
                        First_lastname = created.User.First_lastname,
                        Second_lastname = created.User.Second_lastname,
                        Phone = created.User.Phone,
                        Created_at = created.User.Created_at,
                    },
                    Sell_company = created.Sell_company,
                    Charged_company = created.Charged_company,
                    Currency = created.Currency,
                    Status = created.Status,
                    Pending_balance = created.Pending_balance,
                    Created_at = created.Created_at
                };
                return new Response<InvoiceResponse>()
                {
                    Data = success,
                    Success = true,
                    Message = "Se ingresó la factura exitosamente"
                };
            }
            catch (Exception ex)
            {
                return new Response<InvoiceResponse>()
                {
                    Data = null,
                    Success = false,
                    Message = "Ha ocurrido un error: " + ex.Message
                };
            }
        }

        public async Task<Response<List<InvoiceResponse>>> GetAllInvoices()
        {
            try
            {
                var invoices = await service.GetAllInvoices();
                if (invoices is null || !invoices.Any())
                {
                    return new Response<List<InvoiceResponse>>()
                    {
                        Data = [],
                        Success = false,
                        Message = "No hay facturas registradas en el sistema"
                    };
                }

                var success = invoices.Select(i => new InvoiceResponse()
                {
                    Invoice_id = i.Invoice_id,
                    Consecutive = i.Consecutive,
                    Total_voucher = i.Total_voucher,
                    User = i.User,
                    Sell_company = i.Sell_company,
                    Charged_company = i.Charged_company,
                    Currency = i.Currency,
                    Status = i.Status,
                    Pending_balance = i.Pending_balance,
                    Created_at = i.Created_at,
                }).ToList();

                return new Response<List<InvoiceResponse>>()
                {
                    Data = success,
                    Success = true,
                    Message = "Se obtuvieron todas las facturas de manera exitosa"
                };
            }
            catch (Exception ex)
            {
                return new Response<List<InvoiceResponse>>()
                {
                    Data = null,
                    Success = false,
                    Message = "Ha ocurrido un error: " + ex.Message
                };
            }
        }

        public async Task<Response<List<InvoiceResponse>>> GetInvoicesBySellCompany(int sell_company_id)
        {
            try
            {
                var response = await service.GetInvoicesBySellCompany(sell_company_id);
                if (response is null || response.Count is 0)
                {
                    return new Response<List<InvoiceResponse>>()
                    {
                        Data = null,
                        Success = true,
                        Message = "No hay facturas registradas a nombre de esta compañía"
                    };
                }

                var success = response.Select(i => new InvoiceResponse()
                {
                    Invoice_id = i.Invoice_id,
                    Consecutive = i.Consecutive,
                    Total_voucher = i.Total_voucher,
                    User = i.User,
                    Sell_company = i.Sell_company,
                    Charged_company = i.Charged_company,
                    Currency = i.Currency,
                    Status = i.Status,
                    Pending_balance = i.Pending_balance,
                    Created_at = i.Created_at,
                }).ToList();

                return new Response<List<InvoiceResponse>>()
                {
                    Data = success,
                    Success = true,
                    Message = "Facturas emitidas por la compañía cargadas con éxito"
                };
            }
            catch (Exception ex)
            {
                return new Response<List<InvoiceResponse>>()
                {
                    Data = null,
                    Success = false,
                    Message = $"Ha ocurrido un error en el proceso, inténtelo de nuevo más tarde: {ex.Message}"
                };
            }
        }

        public async Task<Response<List<InvoiceResponse>>> GetInvoicesByClient(string email)
        {
            try
            {
                var found = await service.GetInvoicesByClient(email);
                if (found is null || !found.Any())
                {
                    return new Response<List<InvoiceResponse>>()
                    {
                        Data = [],
                        Success = false,
                        Message = "El cliente no tiene facturas registradas en el sistema"
                    };
                }
                var success = found.Select(i => new InvoiceResponse()
                {
                    Invoice_id = i.Invoice_id,
                    Consecutive = i.Consecutive,
                    Total_voucher = i.Total_voucher,
                    User = i.User,
                    Sell_company = i.Sell_company,
                    Charged_company = i.Charged_company,
                    Currency = i.Currency,
                    Status = i.Status,
                    Pending_balance = i.Pending_balance,
                    Created_at = i.Created_at,
                }).ToList();
                return new Response<List<InvoiceResponse>>()
                {
                    Data = success,
                    Success = true,
                    Message = "Se obtuvieron las facturas del cliente de manera exitosa"
                };
            }
            catch (Exception ex)
            {
                return new Response<List<InvoiceResponse>>()
                {
                    Data = null,
                    Success = false,
                    Message = "Ha ocurrido un error: " + ex.Message
                };
            }
        }

        public async Task<Response<byte[]>> GetInvoicePdf(int invoiceId, CancellationToken ct = default)
        {
            try
            {
                var pdfBytes = await pdfService.GenerateAsync(invoiceId, ct);

                return new Response<byte[]>()
                {
                    Data = pdfBytes,
                    Success = true,
                    Message = "PDF generado exitosamente"
                };
            }
            catch (Exception ex)
            {
                return new Response<byte[]>()
                {
                    Data = null,
                    Success = false,
                    Message = "Ha ocurrido un error: " + ex.Message
                };
            }
        }
    }
}
