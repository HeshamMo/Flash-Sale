using FlashSale.OrderManager.API.Constants;

namespace FlashSale.OrderManager.API.Middlewares
{
    public class AddStaticUserDevelopmentMiddleware
    {
        private readonly RequestDelegate _next;

        private const string UserId =
            "7f9c4b8e-4c6a-4b4a-8e1a-2f6d8c9b1234";

        private const string Username = "admin-test";

        private const string UserRole = "Admin";

        private const string RefreshToken =
            "mock-refresh-token-12345";

        public AddStaticUserDevelopmentMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            context.Request.Headers[GateWayHeaders.UserId] = UserId;
            context.Request.Headers[GateWayHeaders.Username] = Username;
            context.Request.Headers[GateWayHeaders.UserRole] = UserRole;
            context.Request.Headers[GateWayHeaders.RefreshTokenHeader] = RefreshToken;

            await _next(context);
        }
    }
}