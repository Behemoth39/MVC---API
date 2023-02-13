using Microsoft.AspNetCore.Mvc;
using WestCoastEducation.web.Models;
using WestCoastEducation.web.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace WestCoastEducation.web.Controllers;

[Route("course/admin")]
//[Authorize(Roles = "Admin")]
public class CourseAdminController : Controller
{

    /*public async Task<IActionResult> Index()
    {
        return View("Index");
    }

    [HttpGet("create")]
    public IActionResult Create()
    {
        return View("Index");
    }

    [HttpPost("create")]
    public async Task<IActionResult> Create(CoursePostViewModel course)
    {
        return View("Index");
    }

    [HttpGet("edit/{courseId}")]
    public async Task<IActionResult> Edit(int courseId)
    {
        return View("Index");
    }

    [HttpPost("edit/{courseId}")]
    public async Task<IActionResult> Edit(int courseId, CourseUpdateViewModel course)
    {
        return View("Index");
    }

    [Route("delete/{courseId}")]
    public async Task<IActionResult> Delete(int courseId)
    {
        return View("Index");
    }*/
}
