using ShopAppForTest.Models.Api;

namespace ShopAppForTest.Models.Db
{
    public class DbDocumentModel
    {
        public Guid Id { get; set; }
        public CreateDocumentModel CreateDocumentModel { get; set; }

        public List<Guid> AllowedUsers { get; set; }
    }
}
