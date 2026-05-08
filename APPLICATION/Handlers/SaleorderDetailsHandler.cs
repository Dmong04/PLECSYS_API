using APPLICATION.Use_cases.Products_case;
using APPLICATION.Use_cases.SaleOrderDetails_case;
using APPLICATION.Use_cases.SaleOrders_case;
using APPLICATION.Use_cases.Users_case;
using DOMAIN.Entities;
using DOMAIN.Entities.GPS;
using DOMAIN.Interfaces;
using DOMAIN.Interfaces.GPS;
using MongoDB.Driver.GeoJsonObjectModel;

namespace APPLICATION.Handlers
{
    public class SaleOrderDetailsHandler(ISaleOrderDetailsRepository _service, IProductRepository _product,
        ISaleOrderRepository _order, ISupplierRepository _supplier, ISellerLocationTrackingRepository _location)
    {
        public async Task<Response<SaleOrdersResponse>> CreateSaleOrder(SaleOrdersRequest request)
        {
            try
            {
                const decimal TAX_RATE = 13.0m;

                var location = new SellerLocationTracking()
                {
                    SellerId = request.User_email,
                    Timestamp = DateTime.UtcNow,
                    Location = new GeoJsonPoint<GeoJson2DGeographicCoordinates>(
                        new GeoJson2DGeographicCoordinates(request.GPS.Longitude, request.GPS.Latitude))
                };

                await _location.SaveAsync(location);

                var supplier = await _supplier.GetSupplierById(request.Supplier_id);

                if (supplier is null)
                {
                    return new Response<SaleOrdersResponse>()
                    {
                        Data = null,
                        Success = false,
                        Message = "El Cliente seleccionado no se encontró"
                    };
                }

                var order = new SaleOrder()
                {
                    Supplier_id = supplier.Supplier_id,
                    User_email = request.User_email,
                    Company_id = request.Company_id,
                };

                var created_order = await _order.CreateSaleOrder(order);
                if (created_order is null)
                {
                    return new Response<SaleOrdersResponse>()
                    {
                        Data = null,
                        Success = false,
                        Message = "No se pudo crear la orden de compra"
                    };
                }

                var details = new List<SaleOrderDetails>();
                foreach (var item in request.SaleOrders)
                {
                    var product = await _product.GetProductById(item.Product_id);
                    if (product is null)
                    {
                        return new Response<SaleOrdersResponse>()
                        {
                            Data = null,
                            Success = false,
                            Message = "No se encontró el producto seleccionado"
                        };
                    }

                    decimal subtotal = item.Quantity * product.Unit_price;
                    decimal tax = Math.Round(subtotal * (TAX_RATE / 100), 2);
                    decimal total = Math.Round(subtotal + tax, 2);

                    details.Add(new SaleOrderDetails()
                    {
                        Order_id = order.Order_id,
                        Product_id = item.Product_id,
                        Quantity = item.Quantity,
                        Unit_price = product.Unit_price,
                        Tax_rate = TAX_RATE,
                        Tax = tax,
                        Subtotal = subtotal,
                        Total = total,
                    });
                }

                foreach (var detail in details)
                {
                    var saved = await _service.CreateSaleOrderDetails(detail);
                    if (!saved)
                        return new Response<SaleOrdersResponse>()
                        {
                            Data = null,
                            Success = false,
                            Message = "Error al guardar detalles de productos"
                        };
                }

                return new Response<SaleOrdersResponse>()
                {
                    Data = null,
                    Success = true,
                    Message = "Orden de compra creada con éxito"
                };
            }
            catch (Exception ex)
            {
                return new Response<SaleOrdersResponse>()
                {
                    Data = null,
                    Success = true,
                    Message = $"Ha habido un error al procesar la solicitud: {ex.Message}"
                };
            }
        }
    }
}
