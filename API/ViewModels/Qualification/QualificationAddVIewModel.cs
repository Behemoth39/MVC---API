using System.ComponentModel.DataAnnotations;

namespace westcoasteducation.api.ViewModels
{
    public class QualificationAddVIewModel
    {
        [Required(ErrorMessage = "Kompetensområde måste finnas")]
        public string Qualification { get; set; }
    }
}