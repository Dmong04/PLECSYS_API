using APPLICATION.Use_cases.Currencies_case;
using DOMAIN.Entities;
using DOMAIN.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APPLICATION.Handlers
{
    public class CurrencyHandler(ICurrencyRepository service)
    {
        public async Task<Response<CurrencyResponse>> CreateCurrency(CurrencyRequest request)
        {
            try
            {
                var new_currency = new Currency()
                {
                    Currency_code = request.Currency_code,
                    Currency_ISO = request.Currency_ISO,
                    Currency_name = request.Currency_name
                };
                var created = await service.CreateCurrency(new_currency);
                if (created is null)
                {
                    return new Response<CurrencyResponse>()
                    {
                        Data = null,
                        Success = false,
                        Message = "No se ha podido crear la denominación"
                    };
                }

                var success = new CurrencyResponse()
                {
                    Currency_id = created.Currency_id,
                    Currency_ISO = created.Currency_ISO,
                    Currency_code = created.Currency_code,
                    Currency_name = created.Currency_name
                };
                return new Response<CurrencyResponse>()
                {
                    Data = success,
                    Success = true,
                    Message = "Se ha agregado la denominación correctamente al sistema"
                };
            }
            catch (Exception ex)
            {
                return new Response<CurrencyResponse>()
                {
                    Data = null,
                    Success = false,
                    Message = "Ha ocurrido un error al procesar la solicitud: " + ex.Message
                };
            }
        }

        public async Task<Response<List<CurrencyResponse>>> GetAllCurrencies()
        {
            try
            {
                var currencies = await service.GetAllCurrencies();
                if (currencies.Count is 0)
                {
                    return new Response<List<CurrencyResponse>>()
                    {
                        Data = null,
                        Success = false,
                        Message = "El registro de denominaciones está vacío"
                    };
                }

                var success = currencies.Select(c => new CurrencyResponse()
                {
                    Currency_id = c.Currency_id,
                    Currency_ISO = c.Currency_ISO,
                    Currency_code = c.Currency_code,
                    Currency_name = c.Currency_name,
                }).ToList();

                return new Response<List<CurrencyResponse>>()
                {
                    Data = success,
                    Success = true,
                    Message = "Lista de denominaciones desplegada con éxito"
                };
            }
            catch (Exception ex)
            {
                return new Response<List<CurrencyResponse>>()
                {
                    Data = null,
                    Success = false,
                    Message = "Ha ocurrido un error al procesar la solicitud: " + ex.Message
                };
            }
        }

        public async Task<Response<CurrencyResponse>> GetCurrencyById(int currency_id)
        {
            try
            {
                var found = await service.GetCurrencyById(currency_id);
                if (found is null)
                {
                    return new Response<CurrencyResponse>()
                    {
                        Data = null,
                        Success = false,
                        Message = "La denominación consultada no existe en el sistema"
                    };
                }

                var success = new CurrencyResponse()
                {
                    Currency_id = currency_id,
                    Currency_ISO = found.Currency_ISO,
                    Currency_code = found.Currency_code,
                    Currency_name = found.Currency_name
                };
                return new Response<CurrencyResponse>()
                {
                    Data = success,
                    Success = true,
                    Message = "Se obtuvo la denominación consultada con éxito"
                };
            }
            catch (Exception ex)
            {
                return new Response<CurrencyResponse>()
                {
                    Data = null,
                    Success = false,
                    Message = "Ha ocurrido un error al procesar la solicitud: " + ex.Message
                };
            }
        }
    }
}
