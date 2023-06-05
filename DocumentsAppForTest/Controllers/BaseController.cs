using Microsoft.AspNetCore.Mvc;
using ShopAppForTest.ServiceLayer.AuthLayer;
using ILogger = Serilog.ILogger;

namespace ShopAppForTest.Controllers
{
    public abstract class BaseController : ControllerBase
    {
        protected readonly IAuthLayerService _authLayerService;
        private readonly ILogger _logger;

        public BaseController(IAuthLayerService authLayerService, ILogger logger)
        {
            _authLayerService = authLayerService;
            _logger = logger;
        }

        protected string Token => Request.Headers["Token"];

        protected ILogger Logger => _logger;

        protected int BaseDelayInMs = 1;

        protected async Task<Guid> GetUserIdAsync() => (await _authLayerService.GetUserByTokenAsync(Token)).Id;
    }
}
