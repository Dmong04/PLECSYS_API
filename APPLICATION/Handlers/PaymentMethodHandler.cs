using APPLICATION.Use_cases.PaymentMethods_case;
using DOMAIN.Entities;
using DOMAIN.Interfaces;
using MongoDB.Driver.Core.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APPLICATION.Handlers
{
    public class PaymentMethodHandler(IPaymentMethodRepository service)
    {
        public async Task<Response<PaymentMethodResponse>> CreatePaymentMethod(PaymentMethodRequest request)
        {
            try
            {
                var found = await this.GetPaymentMethodByNameAndCode(request);
                if (found.Success)
                {
                    var failed = new Response<PaymentMethodResponse>()
                    {
                        Data = found.Data,
                        Success = false,
                        Message = "El método de pago ya se encuentra registrado en el servidor"
                    };

                    return failed;
                }

                var new_payment_method = new PaymentMethod()
                {
                    Payment_method_name = request.Payment_method_name,
                    Payment_method_code = request.Payment_method_code
                };

                var created = await service.CreatePaymentMethod(new_payment_method);
                if (created is not null)
                {
                    var success = new PaymentMethodResponse()
                    {
                        Payment_method_id = created.Payment_method_id,
                        Payment_method_name = created.Payment_method_name,
                        Payment_method_code = created.Payment_method_code
                    };

                    var response = new Response<PaymentMethodResponse>()
                    {
                        Data = success,
                        Success = true,
                        Message = "Se ingresó el nuevo método de pago con éxito"
                    };

                    return response;
                }

                return new Response<PaymentMethodResponse>()
                {
                    Data = null,
                    Success = false,
                    Message = "Ha ocurrido un error al ingresar el método de pago"
                };
            } catch (Exception ex)
            {
                var exception = new Response<PaymentMethodResponse>()
                {
                    Data = null,
                    Success = false,
                    Message = "Ha ocurrido un error al procesar la solicitud: " + ex.Message
                };

                return exception;
            }
        }

        public async Task<Response<PaymentMethodResponse>> GetPaymentMethodByNameAndCode(PaymentMethodRequest request)
        {
            try
            {
                var found = await service.GetPaymentMethodByNameAndCode(request.Payment_method_name, request.Payment_method_code);
                if (found is null)
                {
                    var failed = new Response<PaymentMethodResponse>()
                    {
                        Data = null,
                        Success = false,
                        Message = "No se ha encontrado el método de pago en el servidor"
                    };

                    return failed;
                }

                var success = new PaymentMethodResponse()
                {
                    Payment_method_id = found.Payment_method_id,
                    Payment_method_name = found.Payment_method_name,
                    Payment_method_code = found.Payment_method_code
                };

                var response = new Response<PaymentMethodResponse>()
                {
                    Data = success,
                    Success = true,
                    Message = "Se ha encontrado el método de pago consultado"
                };

                return response;
            } catch (Exception ex)
            {
                var exception = new Response<PaymentMethodResponse>()
                {
                    Data = null,
                    Success = false,
                    Message = "Ha ocurrido un error al procesar la solicitud: " + ex.Message
                };

                return exception;
            }
        }

        public async Task<Response<List<PaymentMethodResponse>>> GetAllPaymentMethods()
        {
            try
            {
                var paymentMethods = await service.GetAllPaymentPethods();
                if (paymentMethods is null || !paymentMethods.Any())
                {
                    return new Response<List<PaymentMethodResponse>>()
                    {
                        Data = null,
                        Success = false,
                        Message = "´No hay métodos de pago registrados en el servidor"
                    };
                }

                var selected = paymentMethods.Select(pm => new PaymentMethodResponse
                {
                    Payment_method_id = pm.Payment_method_id,
                    Payment_method_name = pm.Payment_method_name,
                    Payment_method_code = pm.Payment_method_code
                }).ToList();

                return new Response<List<PaymentMethodResponse>>
                {
                    Data = selected,
                    Success = true,
                    Message = "Métodos de pago obtenidos correctamente."
                };
            } catch (Exception ex)
            {
                return new Response<List<PaymentMethodResponse>>()
                {
                    Data = null,
                    Success = false,
                    Message = "Ha ocurrido un error: " + ex.Message
                };
            }
        }
    }
}
