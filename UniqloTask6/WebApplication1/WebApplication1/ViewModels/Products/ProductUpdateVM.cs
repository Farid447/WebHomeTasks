using System.ComponentModel.DataAnnotations;
using WebApplication1.Models;

namespace WebApplication1.ViewModels.Products;

public class ProductUpdateVM
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal CostPrice { get; set; }
    public decimal SellPrice { get; set; }
    [Range(0, int.MaxValue)]
    public int Quantity { get; set; }
    [Range(0, 100)]
    public int Discount { get; set; }
    public int CategoryId { get; set; }
    public string? FileUrl { get; set; }
    public IFormFile File { get; set; }
    public IEnumerable<string>? OtherFilesUrls { get; set; }
    public ICollection<IFormFile>? OtherFiles { get; set; }


    public static implicit operator Product(ProductUpdateVM vm)
    {
        return new Product
        {
            Name = vm.Name,
            Description = vm.Description,
            CostPrice = vm.CostPrice,
            SellPrice = vm.SellPrice,
            Quantity = vm.Quantity,
            Discount = vm.Discount,
            CategoryId = vm.CategoryId,
        };
    }
}
