namespace ShopAppForTest.Models.Api
{
    public class BadRequestModel
    {
        public BadRequestStatus ErrorStatus { get; set; }

        public List<string> Errors { get; set; }
    }

    public enum BadRequestStatus
    {
        None,
        ValidationError,
        NotAllowed,
        Error,
    }
}
