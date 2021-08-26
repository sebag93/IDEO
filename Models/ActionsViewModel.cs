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

        [Display(Name = "Nazwa węzła")]
        public string ParentName { get; set; }
    }

    public class EditViewModel
    {
        [Display(Name = "Nazwa")]
        public string Name { get; set; }

        [Display(Name = "Nowa nazwa")]
        public string NewName { get; set; }

        [Display(Name = "Nazwa węzła")]
        public string ParentName { get; set; }

        [Display(Name = "Nazwa nowego węzła")]
        public string NewParentName { get; set; }
    }
}