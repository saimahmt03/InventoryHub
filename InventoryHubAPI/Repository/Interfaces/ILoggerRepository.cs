
namespace InventoryHubAPI.Reporitory.Interfaces
{
    public interface ILoggerRepository
    {
        Task LogRequestAsync(string method, string path, string body);
        Task LogResponseAsync(string method, string path, int statusCode, string body);
    }
}