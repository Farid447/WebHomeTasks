using WebApplication1.Models;

namespace WebApplication1.ViewModels.Commons;

public class HomeVM
{
    public IEnumerable<Slider>? Sliders { get; set; }
    public IEnumerable<Product>? Products { get; set; }

}
