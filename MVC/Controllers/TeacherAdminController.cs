using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text;
using static System.Net.Mime.MediaTypeNames;
using WestCoastEducation.web.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using WestCoastEducation.web.Models;

namespace WestCoastEducation.web.Controllers;

[Route("teacher/admin")]
//[Authorize(Roles = "Admin")]
public class TeacherAdminController : Controller
{
    private readonly IConfiguration _config;
    private readonly string _baseUrl;
    private readonly JsonSerializerOptions _options;
    private readonly IHttpClientFactory _httpClient;

    public TeacherAdminController(IConfiguration config, IHttpClientFactory httpClient)
    {
        _httpClient = httpClient;
        _config = config;
        _baseUrl = _config.GetSection("apiSettings:baseUrl").Value!;
        _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
    }

    public async Task<IActionResult> Index()
    {
        using var client = _httpClient.CreateClient();
        var response = await client.GetAsync($"{_baseUrl}/teachers");

        if (!response.IsSuccessStatusCode) return Content("Ett fel har uppstått!");

        var json = await response.Content.ReadAsStringAsync();

        var teachers = JsonSerializer.Deserialize<IList<TeacherListViewModel>>(json, _options);

        return View("Index", teachers);
    }

    [HttpGet("create")]
    public IActionResult Create()
    {
        var teacher = new TeacherPostViewModel();
        return View("Create", teacher);
    }

    [HttpPost("create")]
    public async Task<IActionResult> Create(TeacherPostViewModel teacher)
    {
        if (!ModelState.IsValid) return View("Create", teacher);

        var model = new
        {
            Age = teacher.Age,
            FirstName = teacher.FirstName,
            LastName = teacher.LastName,
            Email = teacher.Email,
            Phone = teacher.Phone
        };

        using var client = _httpClient.CreateClient();
        var content = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, Application.Json);

        var response = await client.PostAsync($"{_baseUrl}/teachers", content);

        if (response.IsSuccessStatusCode)
        {
            return RedirectToAction(nameof(Index));
        }
        return Content("Done!");
    }

    [HttpGet("edit/{teacherId}")]
    public async Task<IActionResult> Edit(int teacherId)
    {
        var qualificationList = new List<SelectListItem>();

        using var client = _httpClient.CreateClient();
        var response = await client.GetAsync($"{_baseUrl}/teachers/{teacherId}");

        var json = await response.Content.ReadAsStringAsync();
        var teacher = JsonSerializer.Deserialize<TeacherPostViewModel>(json, _options);

        return View("Edit", teacher);
    }

    [HttpPost("edit/{teacherId}")]
    public async Task<IActionResult> Edit(int teacherId, TeacherPostViewModel teacher)
    {
        if (!ModelState.IsValid) return View("Edit", teacher);

        var model = new
        {
            Age = teacher.Age,
            FirstName = teacher.FirstName,
            LastName = teacher.LastName,
            Email = teacher.Email,
            Phone = teacher.Phone
        };

        using var client = _httpClient.CreateClient();
        var content = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, Application.Json);

        var response = await client.PutAsync($"{_baseUrl}/teachers/{teacherId}", content);

        if (response.IsSuccessStatusCode)
        {
            return RedirectToAction(nameof(Index));
        }
        return Content("Done!");
    }

    [Route("delete/{teacherId}")]
    public async Task<IActionResult> Delete(int teacherId)
    {
        using var client = _httpClient.CreateClient();

        var response = await client.DeleteAsync($"{_baseUrl}/teachers/{teacherId}");
        if (response.IsSuccessStatusCode)
        {
            return RedirectToAction(nameof(Index));
        }
        return Content("Done!");
    }
}
