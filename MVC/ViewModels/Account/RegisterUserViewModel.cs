using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WestCoastEducation.web.ViewModels;

public class RegisterUserViewModel
{
    [Required(ErrorMessage = "Email är obligatorskt")]
    [EmailAddress(ErrorMessage = "Ej giltig email adress")]
    [DisplayName("Email")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Lösenord är obligatorskt")]
    [DisplayName("Lösenord")]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [Required(ErrorMessage = "Lösenordet måste bekräftas")]
    [DisplayName("Bekräfta ösenord")]
    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "Felaktigt lösenord")]
    public string ConfirmPassword { get; set; }
}
