using APPLICATION.Use_cases.GPS.TrackingLocation_case;
using DOMAIN.Entities.GPS;
using DOMAIN.Interfaces.GPS;
using MongoDB.Driver.GeoJsonObjectModel;

namespace APPLICATION.Handlers.GPS
{
    public class SellerLocationTrackingHandler(ISellerLocationTrackingRepository repository)
    {
        public async Task<Response<SaveLocationResponse>> SaveLocationAsync(SaveLocationRequest request)
        {
            try
            {
                var location = new SellerLocationTracking
                {
                    SellerId = request.SellerId,
                    Timestamp = DateTime.UtcNow,
                    Location = new GeoJsonPoint<GeoJson2DGeographicCoordinates>(
                        new GeoJson2DGeographicCoordinates(request.Longitude, request.Latitude)
                    )
                };

                await repository.SaveAsync(location);

                return new Response<SaveLocationResponse>()
                {
                    Data = new SaveLocationResponse(),
                    Success = true,
                    Message = "Ubicación guardada correctamente"
                };
            }
            catch (Exception ex)
            {
                return new Response<SaveLocationResponse>()
                {
                    Data = null,
                    Success = false,
                    Message = "Error al guardar la ubicación: " + ex.Message
                };
            }
        }
    }

}
