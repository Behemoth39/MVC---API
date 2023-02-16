using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text;
using static System.Net.Mime.MediaTypeNames;
using WestCoastEducation.web.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using WestCoastEducation.web.Models;

namespace WestCoastEducation.web.Controllers;

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

        var students = JsonSerializer.Deserialize<IList<StudentListViewModel>>(json, _options);

        return View("Index", students);
    }

    [HttpGet("create")]
    public async Task<IActionResult> Create()
    {
        var courseList = new List<SelectListItem>();

        using var client = _httpClient.CreateClient();

        var response = await client.GetAsync($"{_baseUrl}/courses");
        if (!response.IsSuccessStatusCode) return Content("Ett fel har uppstått!");
        var json = await response.Content.ReadAsStringAsync();
        var courses = JsonSerializer.Deserialize<List<CourseModel>>(json, _options);

        foreach (var course in courses)
        {
            courseList.Add(new SelectListItem { Value = course.CourseTitle, Text = course.CourseTitle });
        }

        var student = new StudentPostViewModel();
        student.Courses = courseList;
        return View("Create", student);
    }

    [HttpPost("create")]
    public async Task<IActionResult> Create(StudentPostViewModel student)
    {
        if (!ModelState.IsValid) return View("Create", student);

        var model = new
        {
            Age = student.Age,
            FirstName = student.FirstName,
            LastName = student.LastName,
            Email = student.Email,
            Phone = student.Phone,
            Course = student.Course
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
        var courseList = new List<SelectListItem>();

        using var client = _httpClient.CreateClient();
        var response = await client.GetAsync($"{_baseUrl}/students/{studentId}");

        var json = await response.Content.ReadAsStringAsync();
        var student = JsonSerializer.Deserialize<StudentPostViewModel>(json, _options);

        response = await client.GetAsync($"{_baseUrl}/courses");

        json = await response.Content.ReadAsStringAsync();
        var courses = JsonSerializer.Deserialize<List<CourseModel>>(json, _options);

        foreach (var course in courses)
        {
            courseList.Add(new SelectListItem { Value = course.CourseTitle, Text = course.CourseTitle });
        }

        student.Courses = courseList;
        return View("Edit", student);
    }

    [HttpPost("edit/{studentId}")]
    public async Task<IActionResult> Edit(int studentId, StudentPostViewModel student)
    {
        if (!ModelState.IsValid) return View("Edit", student);

        var model = new
        {
            Age = student.Age,
            FirstName = student.FirstName,
            LastName = student.LastName,
            Email = student.Email,
            Phone = student.Phone,
            Course = student.Course
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
