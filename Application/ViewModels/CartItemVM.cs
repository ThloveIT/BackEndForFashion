using System.ComponentModel.DataAnnotations;

namespace BackEndForFashion.Application.ViewModels
{
    public class CartItemVM
    {
        [Required]
        public Guid ProductId { get; set; }
        [Required]
        public int Quantity { get; set; }

    }
}
