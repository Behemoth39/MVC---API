using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text;
using static System.Net.Mime.MediaTypeNames;
using WestCoastEducation.web.ViewModels;

namespace WestCoastEducation.web.Controllers;

[Route("student/admin")]
//[Authorize(Roles = "Admin")]
public class StudentAdminController : Controller
{
    private readonly IConfiguration _config;
    private readonly string _baseUrl;
    private readonly JsonSerializerOptions _options;
    private readonly IHttpClientFactory _httpClient;

    public StudentAdminController(IConfiguration config, IHttpClientFactory httpClient)
    {
        _httpClient = httpClient;
        _config = config;
        _baseUrl = _config.GetSection("apiSettings:baseUrl").Value!;
        _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
    }

    public async Task<IActionResult> Index()
    {
        using var client = _httpClient.CreateClient();
        var response = await client.GetAsync($"{_baseUrl}/students");

        if (!response.IsSuccessStatusCode) return Content("Ett fel har uppstått!");

        var json = await response.Content.ReadAsStringAsync();

        var students = JsonSerializer.Deserialize<IList<PersonViewModel>>(json, _options);

        return View("Index", students);
    }

    [HttpGet("create")]
    public IActionResult Create()
    {
        var course = new PersonPostViewModel();
        return View("Create", course);
    }

    [HttpPost("create")]
    public async Task<IActionResult> Create(PersonPostViewModel student)
    {
        if (!ModelState.IsValid) return View("Create", student);

        var model = new
        {
            Age = student.Age,
            FirstName = student.FirstName,
            LastName = student.LastName,
            Email = student.Email,
            Phone = student.Phone
        };

        using var client = _httpClient.CreateClient();
        var content = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, Application.Json);

        var response = await client.PostAsync($"{_baseUrl}/students", content);

        if (response.IsSuccessStatusCode)
        {
            return RedirectToAction(nameof(Index));
        }
        return Content("Done!");
    }

    [HttpGet("edit/{studentId}")]
    public async Task<IActionResult> Edit(int studentId)
    {
        using var client = _httpClient.CreateClient();
        var response = await client.GetAsync($"{_baseUrl}/students/{studentId}");

        var json = await response.Content.ReadAsStringAsync();
        var student = JsonSerializer.Deserialize<PersonPostViewModel>(json, _options);

        return View("Edit", student);
    }

    [HttpPost("edit/{studentId}")]
    public async Task<IActionResult> Edit(int studentId, PersonPostViewModel student)
    {
        if (!ModelState.IsValid) return View("Edit", student);

        var model = new
        {
            Age = student.Age,
            FirstName = student.FirstName,
            LastName = student.LastName,
            Email = student.Email,
            Phone = student.Phone
        };

        using var client = _httpClient.CreateClient();
        var content = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, Application.Json);

        var response = await client.PutAsync($"{_baseUrl}/students/{studentId}", content);

        if (response.IsSuccessStatusCode)
        {
            return RedirectToAction(nameof(Index));
        }
        return Content("Done!");
    }

    [Route("delete/{studentId}")]
    public async Task<IActionResult> Delete(int studentId)
    {
        using var client = _httpClient.CreateClient();

        var response = await client.DeleteAsync($"{_baseUrl}/students/{studentId}");
        if (response.IsSuccessStatusCode)
        {
            return RedirectToAction(nameof(Index));
        }
        return Content("Done!");
    }
}
