using InventoryHubAPI.Reporitory.Interfaces;
using InventoryHubAPI.Service.Interfaces;

namespace InventoryHubAPI.Service.Implementations
{
    internal class LoggerService : ILoggerService
    {
        private readonly ILoggerRepository _repository;
        public LoggerService(ILoggerRepository repository)
        {
            _repository = repository;
        }

        public void LogRequest(string method, string path, string body)
        {
            _repository.LogRequestAsync(method, path, body).Wait();
        }

        public void LogResponse(string method, string path, int statusCode, string body)
        {
            _repository.LogResponseAsync(method, path, statusCode, body).Wait();
        }
    }    
}