using Microsoft.AspNetCore.Mvc;

namespace WestCoastEducation.web.Controllers;

[Route("admin")]
public class AdminController : Controller
{
       

    public IActionResult Index()
    {
        return View();
    }

}