using ShopAppForTest.Models;
using ShopAppForTest.ServiceLayer.AuthLayer;

namespace ShopAppForTest.Middlewares
{
    public class CheckTokenMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly string _tokenKey;
        private readonly IAuthLayerService _authLayerService;

        public CheckTokenMiddleware(RequestDelegate next, IAuthLayerService authLayerService, string tokenKey)
        {
            _next = next;
            _authLayerService = authLayerService;
            _tokenKey = tokenKey;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var isExistToken = context.Request.Headers.TryGetValue(_tokenKey, out var token);

            if (!isExistToken)
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Invalid User Key");
                return;
            }
            else
            {
                Guid id = Guid.Empty;
                try
                {
                    var checkUserByToken = await _authLayerService.GetUserByTokenAsync(token);
                    id = checkUserByToken.Id;
                }
                catch (Exception ex)
                {
                    context.Response.StatusCode = 401;
                    await context.Response.WriteAsync("Invalid User Key");
                    return;
                }

                if (id != Guid.Empty)
                {
                    await _next.Invoke(context);
                }
                else
                {
                    context.Response.StatusCode = 401;
                    await context.Response.WriteAsync("Invalid User Key");
                    return;
                }
            }
        }
    }
}
