using System.ComponentModel.DataAnnotations;
using System.Drawing.Drawing2D;

namespace TaaskUniqlo.Models;

public class Product :BaseEntity
{

    [MaxLength(32)]
    public string Name { get; set; } = null!;
    [MaxLength(200)]
    public string Description { get; set; }
    public string Image { get; set; } = null!;
    [Range(0, int.MaxValue)]
    public int PCount { get; set; }
    [DataType("decimal(18,2)")]
    public decimal CostPrice { get; set; }
    [DataType("decimal(18,2)")]
    public decimal SellPrice { get; set; }
    [Range(0, 100)]
    public int Discount { get; set; }
    public int? BrandId { get; set; }
    public Brand? Brand { get; set; }
}
