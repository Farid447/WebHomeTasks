using System.ComponentModel.DataAnnotations;

namespace UniqloTask3.ViewModel.Sliders;

public class SCreateVM
{
    [MaxLength(32,ErrorMessage = "olcu 32-den kicik olmalidir")] [Required]
    public string Title {  get; set; }
    [MaxLength(64, ErrorMessage = "olcu 64-den kicik olmalidir")] [Required]
    public string Subtitle { get; set; }
    public string?  Link {  get; set; }
    [Required(ErrorMessage = "file daxil edin")]
    public IFormFile File { get; set; }
}
