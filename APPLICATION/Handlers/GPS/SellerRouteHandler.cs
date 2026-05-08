using APPLICATION.Use_cases.GPS.SellerRoutes_case;
using DOMAIN.Entities.GPS;
using DOMAIN.Interfaces.GPS;
using MongoDB.Driver.GeoJsonObjectModel;

namespace APPLICATION.Handlers.GPS
{
    public class SellerRouteHandler(ISellerRouteRepository service)
    {
        public async Task<Response<List<SellerRouteResponse>>> GetSellerRoutesBySellerId(string seller_id)
        {
            try
            {
                var seller_routes = await service.GetSellerRoutesBySellerId(seller_id);
                if (seller_routes.Count is 0)
                {
                    return new Response<List<SellerRouteResponse>>()
                    {
                        Data = [],
                        Success = false,
                        Message = "No hay rutas para este vendedor"
                    };
                }

                var success = seller_routes.Select(s => new SellerRouteResponse
                {
                    Id = s.Id,
                    Seller_id = s.Seller_id,
                    Timestamp = s.Timestamp,
                    Start_location_name = s.Start_location_name,
                    Start_location_points = s.Start_location_points,
                    End_location_name = s.End_location_name,
                    End_location_points = s.End_location_points
                }).ToList();

                return new Response<List<SellerRouteResponse>>()
                {
                    Data = success,
                    Success = true,
                    Message = "Lista de rutas por vendedor obtenidas con éxito"
                };
            }
            catch (Exception ex)
            {
                return new Response<List<SellerRouteResponse>>()
                {
                    Data = null,
                    Success = false,
                    Message = "Hubo un error al procesar la solicitud: " + ex.Message
                };
            }
        }

        public async Task<Response<SellerRouteResponse>> RegisterSellerRoute(SellerRouteRequest request)
        {
            try
            {
                var start_location = new GeoJsonPoint<GeoJson2DGeographicCoordinates>(
                new GeoJson2DGeographicCoordinates(request.Start_location.Coordinates[0], request.Start_location.Coordinates[1]));

                var end_location = new GeoJsonPoint<GeoJson2DGeographicCoordinates>(
                    new GeoJson2DGeographicCoordinates(request.End_location.Coordinates[0], request.End_location.Coordinates[1]));

                var new_seller_route = new SellerRoute()
                {
                    Seller_id = request.Seller_id,
                    Timestamp = request.Timestamp,
                    Start_location_name = request.Start_location_name,
                    Start_location_points = start_location,
                    End_location_name = request.End_location_name,
                    End_location_points = end_location,
                };

                var created = await service.RegisterSellerRoute(new_seller_route);
                if (created is null)
                {
                    return new Response<SellerRouteResponse>()
                    {
                        Data = null,
                        Success = false,
                        Message = "No se pudo registrar la ruta de vendedor en el sistema"
                    };
                }
                var success = new SellerRouteResponse()
                {
                    Id = created.Id,
                    Seller_id = created.Seller_id,
                    Timestamp = created.Timestamp,
                    Start_location_name = created.Start_location_name,
                    Start_location_points = created.Start_location_points,
                    End_location_name = created.End_location_name,
                    End_location_points = created.End_location_points,
                };

                return new Response<SellerRouteResponse>()
                {
                    Data = success,
                    Success = true,
                    Message = "Se registró la ruta de vendedor exitosamente al sistema"
                };
            }
            catch (Exception ex)
            {
                return new Response<SellerRouteResponse>()
                {
                    Data = null,
                    Success = false,
                    Message = "Ha habido un error al procesar la solicitud: " + ex.Message
                };
            }
        }
    }
}
