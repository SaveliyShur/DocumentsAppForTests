using ShopAppForTest.Models.Api;

namespace ShopAppForTest.ServiceLayer.AuthLayer
{
    public interface IAuthLayerService
    {
        Task<string> UserAuthAsync(string userName, string password);

        Task<Guid> RegisterNewUserAsync(CreateUserModel user);

        Task<(Guid Id, CreateUserModel User)> GetUserByTokenAsync(string token);

        Task<bool> DeleteUserById(Guid id);

        Task<CreateUserModel> ChangeUserAsync(Guid id, CreateUserModel user);
    }
}
