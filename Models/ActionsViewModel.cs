using System.ComponentModel.DataAnnotations;

namespace IDEO.Models
{
    public class DeleteViewModel
    {
        public string Name { get; set; }
    }

    public class AddViewModel
    {
        [Required]
        [Display(Name = "Nazwa")]
        public string Name { get; set; }

        [Display(Name = "Nazwa rodzica")]
        public string ParentName { get; set; }
    }
}