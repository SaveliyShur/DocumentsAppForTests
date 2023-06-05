namespace ShopAppForTest.Middlewares
{
    public class CheckRegisterTokenMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly string _tokenKey;
        private readonly string _tokenValue;

        public CheckRegisterTokenMiddleware(RequestDelegate next, string tokenKey, string registerToken)
        {
            _next = next;
            _tokenKey = tokenKey;
            _tokenValue = registerToken;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var isExistToken = context.Request.Headers.TryGetValue(_tokenKey, out var token);

            if (!isExistToken || token != _tokenValue)
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Invalid User Key");
                return;
            }
            else
            {
                await _next.Invoke(context);
            }
        }
    }
}
