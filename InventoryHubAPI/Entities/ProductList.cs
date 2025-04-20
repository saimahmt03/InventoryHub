
using InventoryHubAPI.DTO.Result;

namespace InventoryHubAPI.Entities
{
    internal class ProductList : BaseResult
    {
        public List<Product> Products {get; set;} = new List<Product>();
    }
}