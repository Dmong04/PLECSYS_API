using APPLICATION.Use_cases.Products_case;
using DOMAIN.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APPLICATION.Handlers
{
    public class ProductHandler(IProductRepository _service)
    {
        public async Task<Response<List<ProductResponse>>> GetAllProducts()
        {
            try
            {
                var products = await _service.GetAllProducts();

                if (products.Count is 0)
                {
                    return new Response<List<ProductResponse>>()
                    {
                        Data = [],
                        Success = true,
                        Message = "No se encuentran productos registrados en el sistema"
                    };
                }

                var success = products.Select(p => new ProductResponse()
                {
                    Product_id = p.Product_id,
                    Product_name = p.Product_name,
                    Unit_price = p.Unit_price
                }).ToList();

                return new Response<List<ProductResponse>>()
                {
                    Data = success,
                    Success = true,
                    Message = "Listado de productos obtenido con éxito"
                };
            }
            catch (Exception ex)
            {
                return new Response<List<ProductResponse>>()
                {
                    Data = [],
                    Success = false,
                    Message = $"Hubo un error al procesar la solicitud: {ex.Message}"
                };
            }
        }

        public async Task<Response<List<ProductResponse>>> GetProductsByName(string query)
        {
            try
            {
                var products = await _service.GetProductsByName(query);

                if (products.Count is 0)
                {
                    return new Response<List<ProductResponse>>()
                    {
                        Data = [],
                        Success = true,
                        Message = "No se encuentran productos registrados en el sistema"
                    };
                }

                var success = products.Select(p => new ProductResponse()
                {
                    Product_id = p.Product_id,
                    Product_name = p.Product_name,
                    Unit_price = p.Unit_price
                }).ToList();

                return new Response<List<ProductResponse>>()
                {
                    Data = success,
                    Success = true,
                    Message = "Listado de productos obtenido con éxito"
                };
            }
            catch (Exception ex)
            {
                return new Response<List<ProductResponse>>()
                {
                    Data = [],
                    Success = false,
                    Message = $"Hubo un error al procesar la solicitud: {ex.Message}"
                };
            }
        }
    }
}
