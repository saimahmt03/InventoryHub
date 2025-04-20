
namespace InventoryHubClient.DTO
{
    public class ProductList : BaseResponse
    {
        public List<Product> Products { get; set; } = new List<Product>();
    }
}