using ParsePdfApp.Dtos;
using ParsePdfApp.Services;

namespace ParsePdfApp.Middleware
{
    public class AuthCheckMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IServiceScopeFactory _scopeFactory;

        public AuthCheckMiddleware(RequestDelegate next, IServiceScopeFactory scopeFactory)
        {
            _next = next;
            _scopeFactory = scopeFactory;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Path.StartsWithSegments("/auth"))
            {
                Console.WriteLine($"Middleware executed for path auth");
                await _next(context);
                return;
            }

            using IServiceScope scope = _scopeFactory.CreateScope();
            TokenService tokenService = scope.ServiceProvider.GetRequiredService<TokenService>();

            try
            {
                // Get Authorization header
                string authorizationHeader = context.Request.Headers.Authorization.ToString();
                if (string.IsNullOrEmpty(authorizationHeader))
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await context.Response.WriteAsJsonAsync(new
                    {
                        error = "Unauthorized",
                        message = "Missing Authorization header."
                    });
                    return;
                }

                // Extract the token
                string accessToken = authorizationHeader.Split(" ")[1];
                if (string.IsNullOrEmpty(accessToken))
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await context.Response.WriteAsJsonAsync(new
                    {
                        error = "Unauthorized",
                        message = "Missing access token."
                    });
                    return;
                }

                // Validate the token
                UserDto userData = tokenService.ValidateAccessToken(accessToken);
                if (userData == null)
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await context.Response.WriteAsJsonAsync(new
                    {
                        error = "Unauthorized",
                        message = "Invalid access token."
                    });
                    return;
                }

                // Store user data in HttpContext for later use
                context.Items["User"] = userData;
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                Console.WriteLine($"Error: {ex}");
                await context.Response.WriteAsJsonAsync(new
                {
                    error = "Unauthorized",
                    message = "Token validation failed.",
                    details = ex.Message // Detailed exception message
                });
                return;
            }

            // Continue to the next middleware or endpoint
            await _next(context);
        }
    }
}
