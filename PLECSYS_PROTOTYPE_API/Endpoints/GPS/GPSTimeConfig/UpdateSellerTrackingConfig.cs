using APPLICATION.Handlers.GPS;
using FastEndpoints;
using PLECSYS_PROTOTYPE_API.API_response;
using static APPLICATION.Use_cases.GPS.TrackingConfigModels_case.TrackingConfigModels;

namespace PLECSYS_PROTOTYPE_API.Endpoints.GPS.GPSTimeConfig
{
    public class UpdateSellerTrackingConfig(SellerTrackingConfigHandler _handler)
    : Endpoint<UpdateTrackingConfigRequest, TrackingConfigResponse>
    {
        public override void Configure()
        {
            Put("api/v1/plecsys/gps/seller/tracking-config");
            AllowAnonymous(); 
        }

        public override async Task HandleAsync(UpdateTrackingConfigRequest request, CancellationToken ct)
        {
            try
            {
                var result = await _handler.UpdateConfigAsync(request);
                await ResponseBuilder.BuildResponse(
                    result.Data,
                    result.Success,
                    result.Message,
                    HttpContext,
                    result.Success ? StatusCodes.Status200OK : StatusCodes.Status400BadRequest,
                    ct
                );
            }
            catch (Exception ex)
            {
                await ResponseBuilder.BuildResponse<TrackingConfigResponse>(
                    null, false,
                    "Error al actualizar configuración: " + ex.Message,
                    HttpContext,
                    StatusCodes.Status500InternalServerError,
                    ct
                );
            }
        }
    }
}
