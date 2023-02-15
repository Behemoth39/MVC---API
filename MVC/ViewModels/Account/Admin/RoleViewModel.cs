using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WestCoastEducation.web.ViewModels;

public class RoleViewModel
{
    [Required(ErrorMessage = "Namn på rollen saknas")]
    [DisplayName("Roll")]
    public string RoleName { get; set; }
}
