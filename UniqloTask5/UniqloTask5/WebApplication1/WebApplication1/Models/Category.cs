using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class Category : BaseEntity
    {
        [Required(ErrorMessage = "Kateqoriyanın adını qeyd edin")]
        public string Name { get; set; }
        public ICollection<Product>? Products { get; set; }
    }
}
