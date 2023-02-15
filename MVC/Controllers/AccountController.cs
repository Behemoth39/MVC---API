using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using WestCoastEducation.web.Models;
using WestCoastEducation.web.ViewModels;

namespace WestCoastEducation.web.Controllers;

[Route("account")]
public class AccountController : Controller
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, RoleManager<IdentityRole> roleManager)
    {
        _roleManager = roleManager;
        _signInManager = signInManager;
        _userManager = userManager;
    }

    [HttpGet("{returnUrl}")]
    public IActionResult Login([FromQuery] string returnUrl)
    {
        var model = new LoginViewModel();
        if (returnUrl is null) returnUrl = "/home";
        ViewBag.returnUrl = returnUrl; // rewrite to fix Post below
        return View("Login", model);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginViewModel model, string returnUrl)
    {
        try
        {
            if (returnUrl is null) returnUrl = "/home";  // this should have not be here 
            ViewBag.returnUrl = returnUrl;               // this should have not be here 

            if (!ModelState.IsValid) return View(model);

            var result = await _signInManager.PasswordSignInAsync(model.UserName!, model.Password!, false, false);

            if (result.Succeeded)
            {
                return Redirect(returnUrl);
            }
            if (result.IsNotAllowed)
            {
                ModelState.AddModelError("Login", "Gick inte att logga in");
            }
            if (result.IsLockedOut)
            {
                ModelState.AddModelError("Login", "Kontot ät låst");
            }

            ModelState.AddModelError("Login", "Något gick fel, kontrollera användarenamn och lösenord!");

            return View("Login", model);
        }
        catch (Exception ex)
        {
            var errorModel = new ErrorModel
            {
                ErrorTitle = "Fel vid inloggning",
                ErrorMessage = ex.Message
            };
            return View("_Error", errorModel);
        }
    }

    [HttpGet("register")]
    public IActionResult Register()
    {
        var registerModel = new RegisterUserViewModel();
        return View("Register", registerModel);
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterUserViewModel model)
    {
        if (!ModelState.IsValid) return View("Register", model);

        var user = new IdentityUser
        {
            UserName = model.Email,
            Email = model.Email
        };

        var result = await _userManager.CreateAsync(user, model.Password!);

        if (result.Succeeded)
        {
            result = await _userManager.AddToRoleAsync(user, "Admin");

            if (result.Succeeded)
            {
                return RedirectToRoute(new { controller = "Courses", action = "Index" });
            }
        }
        else
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("Register", error.Description);
            }
        }
        return View("Register", model);
    }

    [HttpGet("admin/roles")]
    public IActionResult CreateRole()
    {
        var model = new RoleViewModel();
        return View("CreateRole", model);
    }

    [HttpPost("admin/roles")]
    public async Task<IActionResult> CreateRole(RoleViewModel model)
    {
        if (!ModelState.IsValid) return View("CreateRole", model);

        var role = new IdentityRole(model.RoleName!);
        var result = await _roleManager.CreateAsync(role);

        if (result.Succeeded) RedirectToAction(nameof(CreateRole));

        foreach (var error in result.Errors)
        {
            ModelState.AddModelError("CreateRole", error.Description);
        }

        return View("CreateRole", model);
    }

    [HttpGet("logout")]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToRoute(new { controller = "Home", action = "Index" });
    }
}
