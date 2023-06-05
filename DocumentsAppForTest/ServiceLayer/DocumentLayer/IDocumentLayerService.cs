using ShopAppForTest.Models.Api;

namespace ShopAppForTest.ServiceLayer.DocumentLayer
{
    public interface IDocumentLayerService
    {
        Task<Guid> CreateDocumentAsync(Guid userId, CreateDocumentModel model);
        Task<CreateDocumentModel> GetDocumentByIdAsync(Guid userId, Guid documentId);
        Task<bool> DeleteDocumentByIdAsync(Guid userId, Guid documentId);
        Task<bool> ShareDocumentForOtherUserAsync(Guid userId, Guid otherUserId, Guid documentId);

        Task<List<Guid>> GetAllDocumentsAsync(Guid userId);
    }
}
