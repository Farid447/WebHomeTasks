using TaaskUniqlo.Models;
using System.ComponentModel.DataAnnotations;

namespace TaaskUniqlo.ViewModel.Product;

public class ProductCreateVM
{
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal CostPrice { get; set; }
    public decimal SellPrice { get; set; }
    [Range(0, int.MaxValue)]
    public int Discount { get; set; }
    public int Quantity { get; set; }
    [Range(0, 100)]
    public int BrandId { get; set; }
    public IFormFile File { get; set; }
}