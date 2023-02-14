using Microsoft.AspNetCore.Mvc;
using WestCoastEducation.web.Models;
using WestCoastEducation.web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using System.Text.Json;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace WestCoastEducation.web.Controllers;

[Route("course/admin")]
//[Authorize(Roles = "Admin")]
public class CourseAdminController : Controller
{
    private readonly IConfiguration _config;
    private readonly string _baseUrl;
    private readonly JsonSerializerOptions _options;
    private readonly IHttpClientFactory _httpClient;

    public CourseAdminController(IConfiguration config, IHttpClientFactory httpClient)
    {
        _httpClient = httpClient;
        _config = config;
        _baseUrl = _config.GetSection("apiSettings:baseUrl").Value!;
        _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
    }

    public async Task<IActionResult> Index()
    {
        using var client = _httpClient.CreateClient();
        var response = await client.GetAsync($"{_baseUrl}/courses");

        if (!response.IsSuccessStatusCode) return Content("Ett fel har uppstått!");

        var json = await response.Content.ReadAsStringAsync();

        var courses = JsonSerializer.Deserialize<IList<CourseListViewModel>>(json, _options);

        return View("Index", courses);
    }

    [HttpGet("create")]
    public IActionResult Create()
    {
        var course = new CoursePostViewModel();
        return View("Create", course);
    }

    [HttpPost("create")]
    public async Task<IActionResult> Create(CoursePostViewModel course)
    {
        if (!ModelState.IsValid) return View("Create", course);

        var model = new
        {
            CourseNumber = course.CourseNumber,
            CourseTitle = course.CourseTitle,
            CourseStartDate = course.CourseStartDate,
            CourseEndDate = course.CourseEndDate
        };

        using var client = _httpClient.CreateClient();
        var content = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, Application.Json);

        var response = await client.PostAsync($"{_baseUrl}/courses", content);

        if (response.IsSuccessStatusCode)
        {
            return RedirectToAction(nameof(Index));
        }
        return Content("Done!");
    }

    [HttpGet("edit/{courseId}")]
    public async Task<IActionResult> Edit(int courseId) // skickar inte tillbaka information
    {
        using var client = _httpClient.CreateClient();
        var response = await client.GetAsync($"{_baseUrl}/courses/{courseId}");

        return View("Edit");
    }

    [HttpPost("edit/{courseId}")]
    public async Task<IActionResult> Edit(int courseId, CoursePostViewModel course)
    {
        if (!ModelState.IsValid) return View("Edit", course);

        var model = new
        {
            CourseNumber = course.CourseNumber,
            CourseTitle = course.CourseTitle,
            CourseStartDate = course.CourseStartDate,
            CourseEndDate = course.CourseEndDate
        };

        using var client = _httpClient.CreateClient();
        var content = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, Application.Json);

        var response = await client.PutAsync($"{_baseUrl}/courses/{courseId}", content);

        if (response.IsSuccessStatusCode)
        {
            return RedirectToAction(nameof(Index));
        }
        return Content("Done!");
    }

    [Route("delete/{courseId}")]
    public async Task<IActionResult> Delete(int courseId)
    {
        using var client = _httpClient.CreateClient();

        var response = await client.DeleteAsync($"{_baseUrl}/courses/{courseId}");
        if (response.IsSuccessStatusCode)
        {
            return RedirectToAction(nameof(Index));
        }
        return Content("Done!");
    }
}
