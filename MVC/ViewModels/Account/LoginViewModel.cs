using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WestCoastEducation.web.ViewModels.Account;

public class LoginViewModel
{
    [Required(ErrorMessage = "Användarnamn saknas!")]
    [DisplayName("Användarnamn/Email")]
    public string UserName { get; set; }

    [Required(ErrorMessage = "Lösenord saknas!")]
    [DisplayName("Lösenord")]
    [DataType(DataType.Password)]
    public string Password { get; set; }
}
