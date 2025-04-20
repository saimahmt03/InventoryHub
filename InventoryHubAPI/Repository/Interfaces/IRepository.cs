using InventoryHubAPI.DTO.Result;
using InventoryHubAPI.DTO.Request;
using InventoryHubAPI.Entities;

namespace InventoryHubAPI.Reporitory.Interfaces
{
    public interface IRepository
    {
        Task<BaseResult> AddProductAsync(ProductRequest productRequest);

        Task<BaseResult> UpdateProductAsync(ProductRequest productRequest);

        Task<BaseResult> RemoveProductAsync(ProductRequest productRequest);
        
        Task<GetAllProductResult> GetProductListAsync();
    }
}