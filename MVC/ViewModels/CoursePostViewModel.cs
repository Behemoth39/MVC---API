using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
namespace WestCoastEducation.web.ViewModels;

public class CoursePostViewModel
{
    [Required(ErrorMessage = "Kursnumer är obligatoriskt")]
    [DisplayName("Kursnumer")]
    public string CourseNumber { get; set; } = "";

    [Required(ErrorMessage = "Kurstitle är obligatoriskt")]
    [DisplayName("Kurstitel")]
    public string CourseTitle { get; set; } = "";

    [Required(ErrorMessage = "Kursstart är obligatoriskt")]
    [DisplayName("Kursstart")]
    public DateOnly CourseStartDate { get; set; }

    [Required(ErrorMessage = "Kursslut är obligatoriskt")]
    [DisplayName("Kursslut")]
    public DateOnly CourseEndDate { get; set; }
}
