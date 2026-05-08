using DOMAIN.Entities.GPS;
using DOMAIN.Interfaces.GPS;
using static APPLICATION.Use_cases.GPS.TrackingConfigModels_case.TrackingConfigModels;

namespace APPLICATION.Handlers.GPS
{
    public class SellerTrackingConfigHandler(ISellerTrackingConfigRepository _repository)
    {
        public async Task<Response<TrackingConfigResponse>> GetConfigAsync(string sellerId)
        {
            try
            {
                var config = await _repository.GetBySellerIdAsync(sellerId);

                var interval = config?.IntervalMinutes ?? 30;

                return new Response<TrackingConfigResponse>
                {
                    Data = new TrackingConfigResponse
                    {
                        SellerId = sellerId,
                        IntervalMinutes = interval,
                    },
                    Success = true,
                    Message = "Configuración obtenida correctamente"
                };

            }

            catch (Exception ex)
            {
                return new Response<TrackingConfigResponse>
                {
                    Data = null,
                    Success = false,
                    Message = "Error al obtener la configuración" + ex.Message
                };
            }

        }

        public async Task<Response<TrackingConfigResponse>> UpdateConfigAsync(UpdateTrackingConfigRequest request)
        {
            try
            {
                if (request.IntervalMinutes < 1)
                    return new Response<TrackingConfigResponse>
                    {
                        Data = null,
                        Success = false,
                        Message = "El intervalo debe ser al menos 1 minuto"
                    };

                var config = new SellerTrackingConfig
                {
                    SellerId = request.SellerId,
                    IntervalMinutes = request.IntervalMinutes,
                };

                await _repository.UpsertAsync(config);

                return new Response<TrackingConfigResponse>
                {
                    Data = new TrackingConfigResponse
                    {
                        SellerId = request.SellerId,
                        IntervalMinutes = request.IntervalMinutes,
                    },
                    Success = true,
                    Message = "COnfiguración actualizada correctamente"
                };
            }

            catch (Exception ex)
            {

                return new Response<TrackingConfigResponse>
                {
                    Data = null,
                    Success = false,
                    Message = "Error al actualizar la configuración: " + ex.Message
                };


            }
        }
    }
}
