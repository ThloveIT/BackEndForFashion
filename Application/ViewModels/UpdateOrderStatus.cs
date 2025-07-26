using System.ComponentModel.DataAnnotations;

namespace BackEndForFashion.Application.ViewModels
{
    public class UpdateOrderStatus
    {
        [Required]
        public string Status { get; set; }
    }
}
