using Microsoft.AspNetCore.Mvc;
using ShopAppForTest.Models.Api;
using ShopAppForTest.Models.Api.User;
using ShopAppForTest.ServiceLayer.AuthLayer;

namespace ShopAppForTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : BaseController
    {
        public AuthController(IAuthLayerService authLayerService, Serilog.ILogger logger) 
            : base(authLayerService, logger)
        {
        }

        [HttpPost()]
        [Produces("application/json")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestModel), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<string>> GetUserTokenAsync([FromBody] AuthUserModel userModel)
        {
            await Task.Delay(BaseDelayInMs);

            if (userModel is null)
                return BadRequest(new BadRequestModel()
                {
                    ErrorStatus = BadRequestStatus.ValidationError,
                });

            var newToken = await _authLayerService.UserAuthAsync(userModel.UserName, userModel.Password);

            if (string.IsNullOrEmpty(newToken))
                return BadRequest("Auth failed.");

            return Ok(newToken);
        }
    }
}
