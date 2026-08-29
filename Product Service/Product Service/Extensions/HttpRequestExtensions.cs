
using Product_Service.Domain.Contants;

namespace Product_Service.Extensions
{
    public static class HttpRequestExtensions
    {
        public static string? GetUserId(this HttpRequest request)
        {
            return request.Headers[GateWayHeaders.UserId].FirstOrDefault();
        }

        public static string? GetUsername(this HttpRequest request)
        {
            return request.Headers[GateWayHeaders.Username].FirstOrDefault();
        }

        public static string? GetUserRole(this HttpRequest request)
        {
            return request.Headers[GateWayHeaders.UserRole].FirstOrDefault();
        }
    }
}