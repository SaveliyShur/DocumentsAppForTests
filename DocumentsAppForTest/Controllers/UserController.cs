using Microsoft.AspNetCore.Mvc;
using ShopAppForTest.Models;
using ShopAppForTest.Models.Api;
using ShopAppForTest.Models.Api.User;
using ShopAppForTest.ServiceLayer.AuthLayer;
using ILogger = Serilog.ILogger;

namespace ShopAppForTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : BaseController
    {
        public UserController(IAuthLayerService authLayerService, ILogger logger) 
            : base(authLayerService, logger)
        {
        }

        [HttpPost]
        [Produces("application/json")]
        [ProducesResponseType(typeof(CreatedUserModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestModel), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<CreatedUserModel>> CreateUserAsync([FromBody] CreateUserModel user)
        {
            try
            {
                await Task.Delay(BaseDelayInMs).ConfigureAwait(false);
                if (!ModelState.IsValid)
                {
                    var message = string.Join(" | ", ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage));

                    return BadRequest(new BadRequestModel()
                    {
                        ErrorStatus = BadRequestStatus.ValidationError,
                        Errors = new List<string> { message }
                    });
                }

                if (user.UserPersonalInfo is null)
                    return BadRequest(new BadRequestModel()
                    {
                        ErrorStatus = BadRequestStatus.ValidationError,
                    });

                if (user.UserPersonalInfo.Passport?.Series > 9999 || user.UserPersonalInfo.Passport?.Series < 1000)
                {
                    throw new Exception("Passport series exception");
                }

                if (user.UserPersonalInfo.Passport?.Number > 999999 || user.UserPersonalInfo.Passport?.Number < 100000)
                {
                    throw new Exception("Passport number exception");
                }

                var registerNewUser = await _authLayerService.RegisterNewUserAsync(user);

                return Ok(new CreatedUserModel()
                {
                    Id = registerNewUser,
                    CreateUserModel = user,
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpDelete]
        [Produces("application/json")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestModel), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<bool>> DeleteUserAsync([FromBody] DeleteUserModel user)
        {
            await Task.Delay(BaseDelayInMs);
            
            var isGuid = Guid.TryParse(user.UserId, out var id);

            if (!isGuid)
                return BadRequest(new BadRequestModel()
                {
                    ErrorStatus = BadRequestStatus.ValidationError,
                });

            var myId = await GetUserIdAsync();

            if (myId != id)
            {
                return BadRequest(new BadRequestModel()
                {
                    ErrorStatus = BadRequestStatus.NotAllowed,
                });
            }

            if (!user.UserSecretId.All(char.IsDigit))
            {
                return BadRequest(new BadRequestModel()
                {
                    ErrorStatus = BadRequestStatus.Error,
                    Errors = new List<string>()
                    {
                        "SecretCode should be digital",
                    }
                });
            }

            var isDelete = await _authLayerService.DeleteUserById(id);

            return Ok(isDelete);
        }

        [HttpGet("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(CreatedUserModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestModel), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<CreatedUserModel>> GetUserAsync(string id)
        {
            await Task.Delay(BaseDelayInMs);
            var myId = await GetUserIdAsync();

            var guid = Guid.Parse(id);

            if (myId != guid)
                return BadRequest(new BadRequestModel()
                {
                    ErrorStatus = BadRequestStatus.NotAllowed,
                });

            var user = await _authLayerService.GetUserByTokenAsync(Token);

            return Ok(new CreatedUserModel()
            {
                Id = user.Id,
                CreateUserModel = user.User,
            });
        }

        [HttpPut("{userId}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(CreatedUserModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestModel), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<CreateUserModel>> PutUserAsync(string userId, [FromBody] CreateUserModel user)
        {
            await Task.Delay(BaseDelayInMs);

            if (!ModelState.IsValid)
            {
                return BadRequest(new BadRequestModel()
                {
                    ErrorStatus = BadRequestStatus.ValidationError,
                });
            }

            var myId = await GetUserIdAsync();

            var guid = Guid.Parse(userId);

            if (myId != guid)
                return BadRequest(new BadRequestModel()
                {
                    ErrorStatus = BadRequestStatus.NotAllowed,
                });

            var changeUser = await _authLayerService.ChangeUserAsync(guid, user);

            return Ok(changeUser);
        }
    }
}
