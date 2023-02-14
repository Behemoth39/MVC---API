using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using WestCoastEducation.web.ViewModels;
namespace WestCoastEducation.web.Controllers;

[Route("courses")]
public class CoursesController : Controller
{
    private readonly IConfiguration _config;
    private readonly string _baseUrl;
    private readonly JsonSerializerOptions _options;
    private readonly IHttpClientFactory _httpClient;

    public CoursesController(IConfiguration config, IHttpClientFactory httpClient)
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

    [Route("course/{courseId}")]
    public async Task<IActionResult> Details(int courseId)
    {
        using var client = _httpClient.CreateClient();
        var response = await client.GetAsync($"{_baseUrl}/courses/{courseId}");

        if (!response.IsSuccessStatusCode) return Content("Ett fel har uppstått!");

        var json = await response.Content.ReadAsStringAsync();

        var courses = JsonSerializer.Deserialize<CourseListViewModel>(json, _options);
        return View("course", courses);
    }
}
