using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using WestCoastEducation.web.ViewModels;

namespace MVC.Controllers
{
    [Route("students")]
    public class StudentsController : Controller
    {
        private readonly IConfiguration _config;
        private readonly string _baseUrl;
        private readonly JsonSerializerOptions _options;
        private readonly IHttpClientFactory _httpClient;

        public StudentsController(IConfiguration config, IHttpClientFactory httpClient)
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

        [Route("student/{studentId}")]
        public async Task<IActionResult> Details(int studentId)
        {
            using var client = _httpClient.CreateClient();
            var response = await client.GetAsync($"{_baseUrl}/students/{studentId}");

            if (!response.IsSuccessStatusCode) return Content("Ett fel har uppstått!");

            var json = await response.Content.ReadAsStringAsync();

            var students = JsonSerializer.Deserialize<PersonViewModel>(json, _options);
            return View("student", students);
        }
    }
}