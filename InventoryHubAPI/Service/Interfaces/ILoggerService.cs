
namespace InventoryHubAPI.Service.Interfaces
{
    public interface ILoggerService
    {
        void LogRequest(string method, string path, string body);
        void LogResponse(string method, string path, int statusCode, string body);
    }
}