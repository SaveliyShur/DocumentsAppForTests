using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShopAppForTest.ServiceLayer.AuthLayer;

namespace ShopAppForTest.Controllers
{
    [Route("health")]
    [ApiController]
    public class HealthController : BaseController
    {
        public HealthController(IAuthLayerService authLayerService, Serilog.ILogger logger) 
            : base(authLayerService, logger)
        {
        }

        [HttpGet]
        public async Task<bool> Health()
        {
            await Task.Delay(BaseDelayInMs);
            return true;
        }
    }
}
