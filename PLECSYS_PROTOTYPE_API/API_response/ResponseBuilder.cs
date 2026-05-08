namespace PLECSYS_PROTOTYPE_API.API_response
{
    public class ResponseBuilder
    {
        public static async Task BuildResponse<T>(T? data, bool success, string message,
            HttpContext ctx, int StatusCode, CancellationToken ct)
        {
            var response = new GenericResponse<T>(data, success, message);
            ctx.Response.StatusCode = StatusCode;
            await ctx.Response.WriteAsJsonAsync(response, ct);
        }
    }
}
