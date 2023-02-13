using System.ComponentModel.DataAnnotations;

namespace westcoasteducation.api.ViewModels;

public class StudentAddUpdateViewModel
{
    [Required(ErrorMessage = "Ålder måste finnas")]
    public int Age { get; set; }

    [Required(ErrorMessage = "Förnamn måste finnas")]
    public string FirstName { get; set; }

    [Required(ErrorMessage = "Efternamn måste finnas")]
    public string LastName { get; set; }

    [Required(ErrorMessage = "Email måste finnas")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Telefonnummer måste finnas")]
    public string Phone { get; set; }
}
