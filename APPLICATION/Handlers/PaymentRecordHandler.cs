using APPLICATION.Use_cases.PaymentRecords_case;
using DOMAIN.Entities;
using DOMAIN.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APPLICATION.Handlers
{
    public class PaymentRecordHandler(IPaymentRecordRepository service,
        IInvoiceRepository invoiceService, IInvoiceHistoryRepository historyService)
    {
        public async Task<Response<PaymentRecordResponse>> CreatePaymentRecord(PaymentRecordRequest request)
        {
            try
            {
                // a) Obtener la factura y validar que exista
                var invoice = await invoiceService.GetInvoiceById(request.Source_id);
                if (invoice is null)
                    return new Response<PaymentRecordResponse>
                    {
                        Data = null,
                        Success = false,
                        Message = "No se encontró la factura asociada al pago."
                    };

                // b) Validar que la factura no tenga reclamo activo
                if (invoice.Status == "Con reclamo")
                    return new Response<PaymentRecordResponse>
                    {
                        Data = null,
                        Success = false,
                        Message = "No se puede registrar un pago: la factura tiene un reclamo activo."
                    };

                // c) Validar detalle si el método de pago es OTRO (código 99)
                if (request.Payment_method_id == 99 && string.IsNullOrWhiteSpace(request.Detail_payment_method))
                    return new Response<PaymentRecordResponse>
                    {
                        Data = null,
                        Success = false,
                        Message = "Debe agregar un detalle válido para la opción de método de pago 'OTRO'."
                    };

                // d) Construir el registro de pago
                var new_payment_record = new PaymentRecord
                {
                    Source_id = request.Source_id,
                    Currency_id = request.Currency_id,
                    Payment_method_id = request.Payment_method_id,
                    Detail_payment_method = request.Detail_payment_method,
                    Paid_amount = request.Paid_amount,
                    Payment_date = request.Payment_date,
                    Payment_detail = request.Payment_detail,
                    Third_party_transaction_id = request.Third_party_transaction_id
                };

                // e) Insertar el registro de pago
                var created = await service.CreatePaymentRecord(new_payment_record);
                if (created is null)
                    return new Response<PaymentRecordResponse>
                    {
                        Data = null,
                        Success = false,
                        Message = "Ha habido un error al registrar el pago."
                    };

                // f) Recalcular pending_balance
                var previous_status = invoice.Status;
                var current_pending = invoice.Pending_balance ?? invoice.Total_voucher;
                var new_pending = current_pending - request.Paid_amount;

                if (new_pending < 0)
                    new_pending = 0;

                // g) Determinar nuevo status
                var new_status = new_pending == 0
                    ? "Pagado"
                    : new_pending < invoice.Total_voucher
                        ? "Parcial"
                        : "Pendiente";

                // h) Actualizar la factura
                invoice.Pending_balance = new_pending;
                invoice.Status = new_status;
                await invoiceService.UpdateInvoice(invoice);

                // i) Registrar en InvoiceHistories
                var history = new InvoiceHistory
                {
                    Invoice_id = invoice.Invoice_id,
                    Record_date = DateTime.Now,
                    Action = "Pago registrado",
                    Description = $"Pago de {request.Paid_amount} registrado. " +
                                         $"Saldo anterior: {current_pending} → Saldo nuevo: {new_pending}",
                    User_id = invoice.User_creator_id,
                    Previous_status = previous_status,
                    New_status = new_status,
                    Paid_amount = request.Paid_amount,
                    Pending_balance = new_pending,
                    Payment_record_id = created.Payment_record_id
                };
                await historyService.CreateHistory(history);

                // j) Construir respuesta
                var response = new PaymentRecordResponse
                {
                    Payment_record_id = created.Payment_record_id,
                    Source = created.Source,
                    Currency = created.Currency,
                    Payment_method = created.Payment_method,
                    Detail_payment_method = created.Detail_payment_method,
                    Paid_amount = created.Paid_amount,
                    Payment_date = created.Payment_date,
                    Payment_detail = created.Payment_detail,
                    Third_party_transaction_id = created.Third_party_transaction_id
                };

                return new Response<PaymentRecordResponse>
                {
                    Data = response,
                    Success = true,
                    Message = "Pago registrado con éxito."
                };
            }
            catch (Exception ex)
            {
                return new Response<PaymentRecordResponse>
                {
                    Data = null,
                    Success = false,
                    Message = "Ha ocurrido un error al procesar la solicitud: " + ex.Message
                };
            }
        }

        public async Task<Response<List<PaymentRecordResponse>>> GetAllPaymentRecords()
        {
            try
            {
                var payment_records = await service.GetAllPaymentRecords();
                if (payment_records.Count is 0)
                {
                    return new Response<List<PaymentRecordResponse>>()
                    {
                        Data = [],
                        Success = false,
                        Message = "El sistema no posee registros de pago"
                    };
                }

                var success = payment_records.Select(r => new PaymentRecordResponse()
                {
                    Payment_record_id = r.Payment_record_id,
                    Source = r.Source,
                    Currency = r.Currency,
                    Payment_method = r.Payment_method,
                    Detail_payment_method = r.Detail_payment_method ?? null,
                    Paid_amount = r.Paid_amount,
                    Payment_date = r.Payment_date,
                    Payment_detail = r.Payment_detail,
                    Third_party_transaction_id = r.Third_party_transaction_id
                }).ToList();

                return new Response<List<PaymentRecordResponse>>()
                {
                    Data = success,
                    Success = true,
                    Message = "Listado de registros de pago obtenido con éxito"
                };
            }
            catch (Exception ex)
            {
                return new Response<List<PaymentRecordResponse>>()
                {
                    Data = null,
                    Success = false,
                    Message = "Ha ocurrido un error al procesar la solicitud: " + ex.Message
                };
            }
        }

        public async Task<Response<PaymentRecordResponse>> GetPaymentRecordById(int payment_record_id)
        {
            try
            {
                var found = await service.GetPaymentRecordsById(payment_record_id);
                if (found is null)
                {
                    return new Response<PaymentRecordResponse>()
                    {
                        Data = null,
                        Success = false,
                        Message = "No existe el registro de pago con el identificador recibido"
                    };
                }

                var success = new PaymentRecordResponse()
                {
                    Payment_record_id = payment_record_id,
                    Source = found.Source,
                    Currency = found.Currency,
                    Payment_method = found.Payment_method,
                    Detail_payment_method = found.Detail_payment_method,
                    Paid_amount = found.Paid_amount,
                    Payment_date = found.Payment_date,
                    Payment_detail = found.Payment_detail,
                    Third_party_transaction_id = found.Third_party_transaction_id
                };

                return new Response<PaymentRecordResponse>()
                {
                    Data = success,
                    Success = true,
                    Message = "Registro de pago encontrado"
                };
            }
            catch (Exception ex)
            {
                return new Response<PaymentRecordResponse>()
                {
                    Data = null,
                    Success = false,
                    Message = "Ha ocurrido un error al procesar la solicitud: " + ex.Message
                };
            }
        }
    }
}
