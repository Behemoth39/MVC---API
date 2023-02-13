using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
namespace WestCoastEducation.web.ViewModels;

public class CourseUpdateViewModel
{
    [Required(ErrorMessage = "Kurs id är obligatoriskt")]
    public int CourseId { get; set; }

    [Required(ErrorMessage = "Kursnamn är obligatoriskt")]
    [DisplayName("Kursnamn")]
    public string CourseName { get; set; } = "";

    [Required(ErrorMessage = "Kursnumer är obligatoriskt")]
    [DisplayName("Kursnumer")]    
    public string CourseNumber { get; set; } = "";

    [Required(ErrorMessage = "Kurstitle är obligatoriskt")] 
    [DisplayName("Kurstitel")]
    public string CourseTitle  { get; set; } = "";  

    [Required(ErrorMessage = "Kursstart är obligatoriskt")]
    [DisplayName("Kursstart")]  
    public string CourseStart { get; set; } = "";  

    [Required(ErrorMessage = "Kurslängd är obligatoriskt")]
    [Range(4, 80, ErrorMessage = "Kurslängd är obligatoriskt och måste minst vara 4 veckor")]
    [DisplayName("Kurslängd")]
    public int? CourseLenght { get; set; } 
}
