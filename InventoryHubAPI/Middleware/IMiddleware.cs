
namespace InventoryHubAPI.Middleware
{
    public interface IMiddleware
    {
        Task InvokeAsync(HttpContext context, RequestDelegate next);
    }
}