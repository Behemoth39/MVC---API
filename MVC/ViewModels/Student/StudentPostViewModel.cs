using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WestCoastEducation.web.ViewModels;

public class StudentPostViewModel
{
    [Required(ErrorMessage = "Ålder måste finnas")]
    [DisplayName("Ålder")]
    public int Age { get; set; }

    [Required(ErrorMessage = "Förnamn måste finnas")]
    [DisplayName("Förnamn")]
    public string FirstName { get; set; }

    [Required(ErrorMessage = "Efternamn måste finnas")]
    [DisplayName("Efternamn")]
    public string LastName { get; set; }

    [Required(ErrorMessage = "Email måste finnas")]
    [DisplayName("Email")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Telefonnummer måste finnas")]
    [DisplayName("Telefonnummer")]
    public string Phone { get; set; }

    public string Course { get; set; }
    public List<SelectListItem> Courses { get; set; }
}
