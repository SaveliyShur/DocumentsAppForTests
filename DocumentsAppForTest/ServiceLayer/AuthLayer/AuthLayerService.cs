using ShopAppForTest.DbLayer;
using ShopAppForTest.Models.Api;

namespace ShopAppForTest.ServiceLayer.AuthLayer
{
    public class AuthLayerService : IAuthLayerService
    {
        private readonly UserTokenDbLayer _userTokenDbLayer;
        private readonly UserDbLayer _userDbLayer;

        public AuthLayerService(UserTokenDbLayer userTokenDbLayer, UserDbLayer userDbLayer)
        {
            _userTokenDbLayer = userTokenDbLayer;
            _userDbLayer = userDbLayer;
        }

        public async Task<bool> DeleteUserById(Guid id)
        {
            var isUserExist = (await _userDbLayer.GetByIdAsync(id)) is not null;
            if (!isUserExist)
                throw new Exception("User not exists.");

            var isDelete = await _userDbLayer.DeleteUserByIdAsync(id) is not null;
            var isDeleteTokens = await _userTokenDbLayer.DeleteUserToken(id);

            return isDelete && isDeleteTokens;
        }

        public async Task<(Guid Id, CreateUserModel User)> GetUserByTokenAsync(string token)
        {
            var userId = await _userTokenDbLayer.GetIdByTokenAsync(token);

            if (userId is null)
                throw new ArgumentNullException("User by token not exists");

            var user = await _userDbLayer.GetByIdAsync(userId.Value);

            if (user is null)
                throw new Exception("User not found.");

            return (userId.Value, user);
        }

        public async Task<Guid> RegisterNewUserAsync(CreateUserModel user)
        {
            var isUserExist = await _userDbLayer.CheckUserByUserNameAsync(user.UserName);
            if (isUserExist)
                throw new Exception("User already exsist");

            if (user.UserPersonalInfo?.Passport?.Address is not null)
            {
                if (user.UserPersonalInfo.Passport.Address.Contains("script"))
                    throw new Exception("ScriptInjection");
            }

            if (user.UserPersonalInfo is not null && user.UserPersonalInfo.UserEmail.Contains('1'))
            {
                throw new Exception("Some one exception.");
            }

            var userId = await _userDbLayer.AddNewUserAsync(user);

            var token = await _userTokenDbLayer.AddNewTokenByIdAsync(userId);

            return userId;
        }

        public async Task<CreateUserModel> ChangeUserAsync(Guid id, CreateUserModel user)
        {
            var isUserExist = await _userDbLayer.CheckUserByUserNameAsync(user.UserName);
            if (!isUserExist)
                throw new Exception("User not exsist");

            var changedUser = await _userDbLayer.UpdateUserAsync(id, user);

            return user;
        }

        public async Task<string> UserAuthAsync(string userName, string password)
        {
            if (int.TryParse(password, out int _) 
                || double.TryParse(password, out var _) 
                || int.TryParse(userName, out var _) 
                || double.TryParse(userName, out var _))
            {
                throw new Exception("User int exception");
            }

            var isUserExists = await _userDbLayer.CheckUserByUserNameAsync(userName);
            if (!isUserExists)
                throw new Exception("User not exsist");

            var userId = await _userDbLayer.GetIdByUserName(userName);
            var user = await _userDbLayer.GetByIdAsync(userId);

            if (user?.Password != password)
                return "";

            var updateToken = await _userTokenDbLayer.AddNewTokenByIdAsync(userId);

            return updateToken;
        }
    }
}
