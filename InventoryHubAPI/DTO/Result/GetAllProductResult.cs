

using InventoryHubAPI.DTO.Request;

namespace InventoryHubAPI.DTO.Result
{
    public class GetAllProductResult : BaseResult
    {
        public List<ProductRequest> ProductList {get; set;} = new List<ProductRequest>();
    }
}