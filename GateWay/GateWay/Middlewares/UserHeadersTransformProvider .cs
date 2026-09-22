using Gateway.Domain.Contants;
using System.Security.Claims;
using Yarp.ReverseProxy.Transforms;
using Yarp.ReverseProxy.Transforms.Builder;

public class UserHeadersTransformProvider:ITransformProvider
{
    private const string RequireAuthenticatedUser = "RequireAuthenticatedUser";

    public void ValidateRoute(TransformRouteValidationContext context)
    {
    }

    public void ValidateCluster(TransformClusterValidationContext context)
    {
    }

    public void Apply(TransformBuilderContext context)
    {
        if(context.Route.Metadata is null ||
    !context.Route.Metadata.TryGetValue(
        RequireAuthenticatedUser,
        out var value) ||
    !string.Equals(value, "true", StringComparison.OrdinalIgnoreCase))
        {
            return;
        }

        context.AddRequestTransform(async transformContext =>
        {
            var httpContext = transformContext.HttpContext;
            var user = httpContext.User;

            // Remove headers supplied by the client
            transformContext.ProxyRequest.Headers.Remove(
                GateWayHeaders.UserId);

            transformContext.ProxyRequest.Headers.Remove(
                GateWayHeaders.Username);

            transformContext.ProxyRequest.Headers.Remove(
                GateWayHeaders.UserRole);

            // Make sure the user is authenticated
            if(user.Identity?.IsAuthenticated != true)
            {
                httpContext.Response.StatusCode =
                    StatusCodes.Status401Unauthorized;

                await httpContext.Response.WriteAsJsonAsync(new
                {
                    message = "User is not authenticated."
                });

                httpContext.Abort();

                return;
            }

            // Get claims from JWT
            var userId =
                user.FindFirstValue(ClaimTypes.NameIdentifier);

            var username =
                user.FindFirstValue(ClaimTypes.Name);

            var role =
                user.FindFirstValue(ClaimTypes.Role);

            // Validate claims
            if(string.IsNullOrEmpty(userId) ||
                string.IsNullOrEmpty(username) ||
                string.IsNullOrEmpty(role))
            {
                httpContext.Response.StatusCode =
                    StatusCodes.Status401Unauthorized;

                await httpContext.Response.WriteAsJsonAsync(new
                {
                    message = "Invalid user identity."
                });

                httpContext.Abort();

                return;
            }

            // Add authenticated user information
            transformContext.ProxyRequest.Headers.TryAddWithoutValidation(
                GateWayHeaders.UserId,
                userId);

            transformContext.ProxyRequest.Headers.TryAddWithoutValidation(
                GateWayHeaders.Username,
                username);

            transformContext.ProxyRequest.Headers.TryAddWithoutValidation(
                GateWayHeaders.UserRole,
                role);
        });
    }
}