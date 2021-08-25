using System.ComponentModel.DataAnnotations;

namespace IDEO.Models
{
    public class DeleteViewModel
    {
        [Display(Name = "Wybierz element")]
        [Required]
        public string Name { get; set; }
    }
}