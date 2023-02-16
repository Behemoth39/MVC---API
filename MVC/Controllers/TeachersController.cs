using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using WestCoastEducation.web.ViewModels;

namespace MVC.Controllers
{
    [Route("teachers")]
    public class TeachersController : Controller
    {
        private readonly IConfiguration _config;
        private readonly string _baseUrl;
        private readonly JsonSerializerOptions _options;
        private readonly IHttpClientFactory _httpClient;

        public TeachersController(IConfiguration config, IHttpClientFactory httpClient)
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

        [Route("teacher/{teacherId}")]
        public async Task<IActionResult> Details(int teacherId)
        {
            using var client = _httpClient.CreateClient();
            var response = await client.GetAsync($"{_baseUrl}/teachers/{teacherId}");

            if (!response.IsSuccessStatusCode) return Content("Ett fel har uppstått!");

            var json = await response.Content.ReadAsStringAsync();

            var teachers = JsonSerializer.Deserialize<TeacherListViewModel>(json, _options);
            return View("teacher", teachers);
        }
    }
}