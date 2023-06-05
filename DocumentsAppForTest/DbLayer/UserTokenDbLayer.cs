using System.Collections.Concurrent;

namespace ShopAppForTest.DbLayer
{
    public class UserTokenDbLayer
    {
        private readonly ConcurrentDictionary<Guid, (string Token, Guid Id)> _tokens = new();

        public UserTokenDbLayer()
        {
            var guid = Guid.Parse("1d712bf9-7c30-4d16-bdc4-b27d4f6aa27d");
            var guid2 = Guid.Parse("eb0a70fc-e3ff-4c17-b995-a0af61786e89");
            var guid3 = Guid.Parse("eb0a70fc-e3ff-4c17-b995-a0af61716a89");
            var guid4 = Guid.Parse("aaaa70fc-e3ff-4c17-b995-a0af61716a89");

            _tokens.AddOrUpdate(guid, ("0ae54932fdbc4a4a9c52e7e25a7e8f07", guid), (key, value) => ("0ae54932fdbc4a4a9c52e7e25a7e8f07", guid));
            _tokens.AddOrUpdate(guid2, ("0ae54932fdbc4a4a9c52e7e25a7e8f17", guid2), (key, value) => ("0ae54932fdbc4a4a9c52e7e25a7e8f17", guid2));
            _tokens.AddOrUpdate(guid3, ("0ae54932fdbc4b4a4c52e7e25a7e8f21", guid3), (key, value) => ("0ae54932fdbc4b4a4c52e7e25a7e8f21", guid3));
            _tokens.AddOrUpdate(guid4, ("0ae54932fdbc4b4a4c52e7e25a7e8f22", guid4), (key, value) => ("0ae54932fdbc4b4a4c52e7e25a7e8f22", guid4));
        }

        public async Task<Guid?> GetIdByTokenAsync(string token)
        {
            await Task.Delay(1).ConfigureAwait(false);

            if (_tokens.Values.Any(t => t.Token == token))
                return _tokens.Values.FirstOrDefault(t => t.Token == token).Id;

            return null;
        }

        public async Task<string> AddNewTokenByIdAsync(Guid id)
        {
            await Task.Delay(1).ConfigureAwait(false);
            
            var newToken = Guid.NewGuid().ToString("N") + Guid.NewGuid().ToString("N");

            _tokens.AddOrUpdate(id, (newToken, id), (key, oldValue) => (newToken, id));

            return newToken;
        }

        public async Task<bool> DeleteUserToken(Guid guid)
        {
            await Task.Delay(1).ConfigureAwait(false);

            var isDelete = _tokens.Remove(guid, out var token);

            return isDelete;
        }
    }
}
