using System.ComponentModel.DataAnnotations;

namespace WebApplication1.ViewModels.Sliders;

public class SliderVM
{
    [Required, MaxLength(32)]
    public string Title { get; set; }
    [MaxLength(128)]
    public string Subtitle { get; set; }
    public string? Link { get; set; } = "";
    public string FilePath { get; set; } = "";
    public IFormFile? File { get; set; }
}
