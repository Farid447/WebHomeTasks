using System.ComponentModel.DataAnnotations;

namespace TaaskUniqlo.ViewModel.Brand
{
    public class BrandCreateVM
    {
        [MaxLength(50, ErrorMessage = "olcu maksimum 50 olmalidir")]
        [Required]
        public string Name { get; set; }
    }
}
