using ShopAppForTest.DbLayer;
using ShopAppForTest.Models.Api;

namespace ShopAppForTest.ServiceLayer.DocumentLayer
{
    public class DocumentLayerService : IDocumentLayerService
    {
        private readonly DocumentDbLayer _documentDbLayer;
        private readonly UserDbLayer _userDbLayer;

        public DocumentLayerService(DocumentDbLayer dbLayer, UserDbLayer userDbLayer)
        {
            _documentDbLayer = dbLayer;
            _userDbLayer = userDbLayer;
        }

        public async Task<Guid> CreateDocumentAsync(Guid userId, CreateDocumentModel model)
        {
            if (model.DocumentInBase64.Contains("script"))
                throw new Exception("Script init exception");

            if (string.IsNullOrEmpty(model.Description))
                throw new Exception("Description should be not null");

            if (model.Sha256HashCode.ToLower().Contains("null")
                || model.Sha256HashCode.ToLower().Contains("false")
                || model.Sha256HashCode.ToLower().Contains("true")
                || model.Sha256HashCode.ToLower().Contains("undefined"))
                throw new Exception("Sha256HashCode exception");

            var documentId = await _documentDbLayer.CreateDocumentAsync(model, userId);

            return documentId.Id;
        }

        public async Task<bool> DeleteDocumentByIdAsync(Guid userId, Guid documentId)
        {
            var isUserExist = await _userDbLayer.GetByIdAsync(userId) is not null;

            if (!isUserExist)
            {
                throw new Exception("User doesnt exists");
            }

            var document = await _documentDbLayer.GetDocumentByIdAsync(documentId);

            if (document is null)
            {
                throw new Exception("Document not found");
            }

            if (!document.AllowedUsers.Any(u => userId == u))
            {
                throw new Exception("Not allowed");
            }

            var deleteDocument = await _documentDbLayer.DeleteDocumentByIdAsync(documentId);

            return deleteDocument is not null;
        }

        public async Task<List<Guid>> GetAllDocumentsAsync(Guid userId)
        {
            var isUserExist = await _userDbLayer.GetByIdAsync(userId) is not null;

            if (!isUserExist)
            {
                throw new Exception("User doesnt exists");
            }

            var documents = await _documentDbLayer.GetAllDocumentsForUser(userId);

            return documents.Select(document => document.Id).ToList();
        }

        public async Task<CreateDocumentModel> GetDocumentByIdAsync(Guid userId, Guid documentId)
        {
            var isUserExist = await _userDbLayer.GetByIdAsync(userId) is not null;

            if (!isUserExist)
            {
                throw new Exception("User doesnt exists");
            }

            var document = await _documentDbLayer.GetDocumentByIdAsync(documentId);

            if (document is null)
            {
                throw new Exception("Document not found");
            }

            if (!document.AllowedUsers.Any(u => userId == u))
            {
                throw new Exception("Not allowed");
            }

            return document.CreateDocumentModel;
        }

        public async Task<bool> ShareDocumentForOtherUserAsync(Guid userId, Guid otherUserId, Guid documentId)
        {
            var isUserExist = await _userDbLayer.GetByIdAsync(userId) is not null;
            var isOtherUserExist = await _userDbLayer.GetByIdAsync(otherUserId) is not null;

            if (!isUserExist || !isOtherUserExist)
            {
                throw new Exception("User doesnt exists");
            }

            var document = await _documentDbLayer.GetDocumentByIdAsync(documentId);

            if (document == null)
            {
                throw new Exception($"Not document by id = {documentId}");
            }

            if (!document.AllowedUsers.Any(u => u == userId))
            {
                throw new Exception("Not allowed");
            }

            var tryShare = await _documentDbLayer.GetRightToDocument(otherUserId, documentId);

            return tryShare;
        }
    }
}