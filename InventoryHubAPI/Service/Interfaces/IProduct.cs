using System.Threading.Tasks;
using InventoryHubAPI.DTO.Request;
using InventoryHubAPI.DTO.Result;

namespace InventoryHubAPI.Service.Interfaces
{
    public interface IProduct
    {
        Task<BaseResult> AddProductAsync(ProductRequest productRequest);

        Task<BaseResult> UpdateProductAsync(ProductRequest productRequest);

        Task<BaseResult> RemoveProductAsync(ProductRequest productRequest);

        Task<GetAllProductResult> GetProductListAsync();
    }
}