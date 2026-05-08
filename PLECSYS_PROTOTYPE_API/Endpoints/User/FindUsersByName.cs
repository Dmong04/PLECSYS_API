using APPLICATION.Handlers;
using APPLICATION.Use_cases.Users_case;
using FastEndpoints;
using PLECSYS_PROTOTYPE_API.API_response;

namespace PLECSYS_PROTOTYPE_API.Endpoints.User
{
    public class FindUsersByName(UserHandler _handler) : EndpointWithoutRequest<List<UserResponse>>
    {
        public override void Configure()
        {
            Get("api/v1/plecsys/user/search/{full_name}");
            AllowAnonymous();
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            try
            {
                var full_name = Route<string>("full_name");
                var users = await _handler.GetUsersByName(full_name);
                if (users.Data.Count is 0)
                    await ResponseBuilder.BuildResponse<List<UserResponse>>(users.Data, users.Success,
                        users.Message, HttpContext, StatusCodes.Status404NotFound, ct);
                await ResponseBuilder.BuildResponse<List<UserResponse>>(users.Data, users.Success,
                        users.Message, HttpContext, StatusCodes.Status200OK, ct);
            }
            catch (Exception ex)
            {
                await ResponseBuilder.BuildResponse<List<UserResponse>>(null, false,
                    "Hubo un error al procesar la solicitud: " + ex.Message,
                    HttpContext, StatusCodes.Status500InternalServerError, ct);
            }
        }
    }
}
