using APPLICATION.Handlers.GPS;
using FastEndpoints;
using PLECSYS_PROTOTYPE_API.API_response;
using static APPLICATION.Use_cases.GPS.TrackingConfigModels_case.TrackingConfigModels;

namespace PLECSYS_PROTOTYPE_API.Endpoints.GPS.GPSTimeConfig
{
    public class GetSellerTrackingConfig(SellerTrackingConfigHandler _handler)
        :Endpoint<GetTrackingConfigRequest, TrackingConfigResponse>
    {
        public override void Configure()
        {
            Get("api/v1/plecsys/gps/seller/tracking-config");
            AllowAnonymous();
        }

        public override async Task HandleAsync(GetTrackingConfigRequest request, CancellationToken ct)
        {
            try
            {
                var result = await _handler.GetConfigAsync(request.SellerId);
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
                    "Error al obtener configuración: " + ex.Message,
                    HttpContext,
                    StatusCodes.Status500InternalServerError,
                    ct
                );
            }
        }
    }
}
