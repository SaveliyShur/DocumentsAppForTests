using System.ComponentModel.DataAnnotations;

namespace ShopAppForTest.Models.Api
{
    public class CreateDocumentModel
    {
        [Required]
        [StringLength(100, MinimumLength = 1)]
        public string Title { get; set; }

        public string? Description { get; set; }

        public string? Sha256HashCode { get; set; }

        [Required]
        public string DocumentInBase64 { get; set; }
    }
}
