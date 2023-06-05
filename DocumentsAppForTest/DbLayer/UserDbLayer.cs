using ShopAppForTest.Models.Api;
using System.Collections.Concurrent;

namespace ShopAppForTest.DbLayer
{
    public class UserDbLayer
    {
        private readonly ConcurrentDictionary<Guid, CreateUserModel> _users = new();

        public UserDbLayer()
        {
            var guid = Guid.Parse("1d712bf9-7c30-4d16-bdc4-b27d4f6aa27d");
            var user = new CreateUserModel()
            {
                UserName = "UserName",
                Password = "qwerty123",
                Age = 20,
            };

            var guid2 = Guid.Parse("eb0a70fc-e3ff-4c17-b995-a0af61786e89");
            var user2 = new CreateUserModel()
            {
                UserName = "UserName1",
                Password = "qwerty123",
                Age = 20,
            };

            var guid3 = Guid.Parse("eb0a70fc-e3ff-4c17-b995-a0af61716a89");
            var user3 = new CreateUserModel()
            {
                UserName = "DeleteUser",
                Password = "qwerty123",
                Age = 20,
            };

            var guid4 = Guid.Parse("aaaa70fc-e3ff-4c17-b995-a0af61716a89");
            var user4 = new CreateUserModel()
            {
                UserName = "AuthUser",
                Password = "qwerty123",
                Age = 20,
            };

            _users.AddOrUpdate(guid, user, (key, value) => user);
            _users.AddOrUpdate(guid2, user2, (key, value) => user2);
            _users.AddOrUpdate(guid3, user3, (key, value) => user3);
            _users.AddOrUpdate(guid4, user4, (key, value) => user4);
        }

        public async Task<bool> CheckUserByUserNameAsync(string userName)
        {
            await Task.Delay(1).ConfigureAwait(false);
            return _users.Any(u => u.Value.UserName == userName);
        }

        public async Task<Guid> AddNewUserAsync(CreateUserModel user)
        {
            await Task.Delay(1).ConfigureAwait(false);
            var id = Guid.NewGuid();
            _users.AddOrUpdate(id, user, (key, oldValue) => user);

            return id;
        }
        
        public async Task<Guid> UpdateUserAsync(Guid id, CreateUserModel newUser)
        {
            await Task.Delay(1).ConfigureAwait(false);
            _users.AddOrUpdate(id, newUser, (key, oldValue) => newUser);

            return id;
        }

        public async Task<CreateUserModel?> DeleteUserByIdAsync(Guid id)
        {
            await Task.Delay(1).ConfigureAwait(false);

            _users.Remove(id, out var u);

            return u;
        }

        public async Task<CreateUserModel?> GetByIdAsync(Guid id)
        {
            await Task.Delay(1).ConfigureAwait(false);
            return _users.GetValueOrDefault(id);
        }

        public async Task<Guid> GetIdByUserName(string userName)
        {
            await Task.Delay(1).ConfigureAwait(false);
            var user = _users.First(u => u.Value.UserName == userName);

            return user.Key;
        }
    }
}
