using Microsoft.AspNetCore.Mvc;

namespace WestCoastEducation.web.Controllers;

public class HomeController : Controller
{
        public IActionResult Index()
    {
        return View("Index");
    }

}
