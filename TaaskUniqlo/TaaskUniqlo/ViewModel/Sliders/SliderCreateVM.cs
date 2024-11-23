using System.ComponentModel.DataAnnotations;

namespace TaaskUniqlo.ViewModel.Sliders
{
    public class SliderCreateVM
    {
        [MaxLength(50,ErrorMessage = "olcu maksimum 50 ola biler")] [Required]
        public string Title {  get; set; }
        [MaxLength(50, ErrorMessage = "olcu maksimum 50 ola biler")] [Required]
        public string Subtitle { get; set; }
        [MaxLength(50, ErrorMessage = "olcu maksimum 50 ola biler")]
        [Required]
        public string?  Link {  get; set; }
        [Required(ErrorMessage = "duzgun secin")]
        public IFormFile File { get; set; }
    }
}
