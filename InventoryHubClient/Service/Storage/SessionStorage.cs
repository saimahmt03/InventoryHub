using Blazored.SessionStorage;

namespace InventoryHubClient.Service.Storage
{
    internal class SessionStorage : ISessionStorage
    {
        private readonly ISessionStorageService _sessionStorage;

        public SessionStorage(ISessionStorageService sessionStorage)
        {
            _sessionStorage = sessionStorage;
        }

        public async Task SetItemAsync<T>(string key, T value)
        {
            await _sessionStorage.SetItemAsync(key, value);
        }
        
        public async Task<T> GetItemAsync<T>(string key)
        {
            var result = await _sessionStorage.GetItemAsync<T>(key);
            if (result != null)
            {
                return result;
            }
            return default(T)!;
        }

        public async Task RemoveItemAsync(string key)
        {
            await _sessionStorage.RemoveItemAsync(key);
        }

        public async Task ClearAsync()
        {
            await _sessionStorage.ClearAsync();
        }
    }
}