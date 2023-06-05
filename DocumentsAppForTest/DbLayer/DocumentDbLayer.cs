using ShopAppForTest.Models.Api;
using ShopAppForTest.Models.Db;
using System.Collections.Concurrent;

namespace ShopAppForTest.DbLayer
{
    public class DocumentDbLayer
    {
        private readonly ConcurrentDictionary<Guid, DbDocumentModel> _documents = new();

        public DocumentDbLayer()
        {
            var guid1 = Guid.Parse("492a8111-c781-4d49-a680-d031220e227d");
            var guid2 = Guid.Parse("d0a924df-0039-4cac-8169-c71809b3cafe");

            var user1 = Guid.Parse("1d712bf9-7c30-4d16-bdc4-b27d4f6aa27d");

            var dbDocumentModel1 = new DbDocumentModel()
            {
                Id = guid1,
                AllowedUsers = new List<Guid> { user1 },
                CreateDocumentModel = new CreateDocumentModel()
                {
                    Title = "Title",
                    DocumentInBase64 = "asdasdsadasdasd",
                },
            };

            var dbDocumentModel2 = new DbDocumentModel()
            {
                Id = guid2,
                AllowedUsers = new List<Guid> { user1 },
                CreateDocumentModel = new CreateDocumentModel()
                {
                    Title = "Title",
                    DocumentInBase64 = "asdasdsadasdasd",
                },
            };

            _documents.AddOrUpdate(guid1, dbDocumentModel1, (key, value) => dbDocumentModel1);
            _documents.AddOrUpdate(guid2, dbDocumentModel2, (key, value) => dbDocumentModel2);
        }

        public async Task<List<DbDocumentModel>> GetAllDocumentsForUser(Guid userId)
        {
            await Task.Delay(1).ConfigureAwait(false);
            var documents = _documents
                .Where(doc => doc.Value.AllowedUsers.Contains(userId))
                .Select(doc => doc.Value)
                .ToList();

            return documents;
        }

        public async Task<DbDocumentModel> DeleteDocumentByIdAsync(Guid id)
        {
            await Task.Delay(1).ConfigureAwait(false);
            var isRemove = _documents.Remove(id, out var doc);

            if (!isRemove)
                throw new Exception("Cannot remove document.");

            return doc;
        }

        public async Task<DbDocumentModel> CreateDocumentAsync(CreateDocumentModel document, Guid userId)
        {
            await Task.Delay(1).ConfigureAwait(false);
            var documentInDb = new DbDocumentModel()
            {
                AllowedUsers = new List<Guid> { userId },
                CreateDocumentModel = document,
                Id = Guid.NewGuid(),
            };

            _documents.AddOrUpdate(documentInDb.Id, documentInDb, (key, value) => documentInDb);

            return documentInDb;
        }

        public async Task<bool> GetRightToDocument(Guid userId, Guid documentId)
        {
            await Task.Delay(1).ConfigureAwait(false);
            var isDocumentExists = _documents.ContainsKey(documentId);

            if (!isDocumentExists)
                throw new Exception("Document doesnt exists");

            var document = _documents.GetValueOrDefault(documentId);
            if (document.AllowedUsers.Contains(userId))
            {
                return false;
            }

            document.AllowedUsers.Add(userId);

            _documents.AddOrUpdate(documentId, document, (key, oldValue) => document);

            return true;
        }

        public async Task<DbDocumentModel?> GetDocumentByIdAsync(Guid id)
        {
            await Task.Delay(1).ConfigureAwait(false);
            return _documents.GetValueOrDefault(id);
        }
    }
}
