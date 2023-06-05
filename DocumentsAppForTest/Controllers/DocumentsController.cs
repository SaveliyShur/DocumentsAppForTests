using Microsoft.AspNetCore.Mvc;
using ShopAppForTest.Models.Api;
using ShopAppForTest.Models.Api.Document;
using ShopAppForTest.ServiceLayer.AuthLayer;
using ShopAppForTest.ServiceLayer.DocumentLayer;
using ILogger = Serilog.ILogger;

namespace ShopAppForTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentsController : BaseController
    {
        private readonly IDocumentLayerService _documentLayerService;

        public DocumentsController(IAuthLayerService authLayerService, IDocumentLayerService documentLayerService, ILogger logger)
            : base(authLayerService, logger)
        {
            _documentLayerService = documentLayerService;
        }

        [HttpPost]
        [Produces("application/json")]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestModel), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Guid>> CreateDocumentAsync([FromBody] CreateDocumentModel document)
        {
            await Task.Delay(BaseDelayInMs);
            if (!ModelState.IsValid)
            {
                return BadRequest(new BadRequestModel()
                {
                    ErrorStatus = BadRequestStatus.ValidationError,
                });
            }
            var user = await GetUserIdAsync();
            try
            {
                var createDocument = await _documentLayerService.CreateDocumentAsync(user, document);

                return Ok(createDocument);
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestModel()
                {
                    Errors = new List<string>()
                    {
                        ex.Message,
                    },
                    ErrorStatus = BadRequestStatus.Error,
                });
            }
        }

        [HttpDelete("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestModel), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<bool>> DeleteDocumentAsync(string id)
        {
            await Task.Delay(BaseDelayInMs);
            var user = await GetUserIdAsync();

            var isGuid = Guid.TryParse(id, out var documentId);

            if (!isGuid)
                return BadRequest(new BadRequestModel()
                {
                    ErrorStatus = BadRequestStatus.ValidationError,
                });

            var isDeleted = await _documentLayerService.DeleteDocumentByIdAsync(user, documentId);

            return isDeleted;
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(CreatedDocumentModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestModel), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<CreatedDocumentModel>> GetDocumentByIdAsync(Guid id)
        {
            await Task.Delay(BaseDelayInMs);
            var user = await GetUserIdAsync();

            try
            {
                var document = await _documentLayerService.GetDocumentByIdAsync(user, id);

                return Ok(document);
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestModel()
                {
                    Errors = new List<string>()
                    {
                        ex.Message,
                    },
                    ErrorStatus = BadRequestStatus.Error,
                });
            }
        }

        [HttpPost("{userId}/{documentId}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestModel), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<bool>> SendDocumentToOtherUser(string userId, string documentId)
        {
            await Task.Delay(BaseDelayInMs);
            var user = await GetUserIdAsync();

            var isGuid = Guid.TryParse(userId, out var userIdGuid);
            var isGuidDocumentId = Guid.TryParse(documentId, out var documentIdGuid);

            if (!isGuid || !isGuidDocumentId)
                return BadRequest(new BadRequestModel()
                {
                    ErrorStatus = BadRequestStatus.ValidationError,
                });

            try
            {
                var isSended = await _documentLayerService.ShareDocumentForOtherUserAsync(user, userIdGuid, documentIdGuid);

                return isSended;
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestModel()
                {
                    Errors = new List<string>()
                    {
                        ex.Message,
                    },
                    ErrorStatus = BadRequestStatus.Error,
                });
            }
        }
    }
}
