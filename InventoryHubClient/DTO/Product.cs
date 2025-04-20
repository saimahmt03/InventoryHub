using System.ComponentModel.DataAnnotations;

namespace InventoryHubClient.DTO
{
    public class Product
    {
        public int Id { get; set; } = 0;
        
        [Required(ErrorMessage = "Product name is required.")]
        [RegularExpression(@"^(?!\d+$)(?![^a-zA-Z0-9]+$).+", ErrorMessage = "The field cannot contain only numbers or special characters.")]
        public string Name { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Price is required.")]
        [RegularExpression(@"^\d+$", ErrorMessage = "Price should be integer.")]
        public double Price { get; set; }
        
        [Required(ErrorMessage = "Stock is required.")]
        [RegularExpression(@"^\d+$", ErrorMessage = "Stock should be integer.")]
        public int Stock { get; set; }
    }
}