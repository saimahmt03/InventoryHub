using System.ComponentModel.DataAnnotations;

namespace InventoryHubAPI.DTO.Request
{
    public class ProductRequest
    {
        public int Id { get; set; } = 0;

        [Required(ErrorMessage = "Product name is required.")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Price is required.")]
        public double Price { get; set; }

        [Required(ErrorMessage = "Stock is required.")]
        public int Stock { get; set; }
    }
}