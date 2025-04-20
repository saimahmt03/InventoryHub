using System.ComponentModel.DataAnnotations;

namespace InventoryHubAPI.Entities
{
    internal class Product
    {
        public int Id { get; set; } = 0;

        [Required(ErrorMessage = "Product name is required.")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Price name is required.")]
        public double Price { get; set; }

        [Required(ErrorMessage = "Stock name is required.")]
        public int Stock { get; set; }
    }
}