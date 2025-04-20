using InventoryHubClient.DTO;

namespace InventoryHubClient.Service
{
    public interface IService
    {
        //Task SetProductsAsync(Product product);
        //Task<Product> GetCurrentProductAsync();
        //Task<ProductList> GetProductsAsync();

        Task<BaseResponse> AddProductAsync(Product newProduct);
        Task<BaseResponse> UpdateProductAsync(Product product);
        Task<BaseResponse> RemoveProductAsync(Product product);
        Task<BaseResponse> SearchProductsAsync(string productName);
        Task<ProductList> GetProductListAsync();

        Task NotifyStateChangedAsync();
    }
}