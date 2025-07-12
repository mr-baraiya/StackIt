using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Stacklt.Models;
using System.Text;

namespace PLPRH_MVC.Controllers
{
    public class UserController : Controller
    {
        private readonly HttpClient _httpClient;

        public UserController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri("https://localhost:7007/api/");
        }

        public async Task<IActionResult> User_List()
        {
            var res = await _httpClient.GetAsync("User/All"); // Updated to match your API route
            var json = await res.Content.ReadAsStringAsync();
            var list = JsonConvert.DeserializeObject<List<UserModel>>(json);
            return View(list);
        }

        [HttpGet]
        public async Task<IActionResult> AddEdit(int? id)
        {
            UserModel user;

            if (id == null)
            {
                user = new UserModel();
            }
            else
            {
                var response = await _httpClient.GetAsync($"User/{id}");
                if (!response.IsSuccessStatusCode) return NotFound();

                var json = await response.Content.ReadAsStringAsync();
                user = JsonConvert.DeserializeObject<UserModel>(json);
            }

            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> AddEdit(UserModel user)
        {
            var content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");

            if (user.Id == 0) // Match your model's property name
            {
                await _httpClient.PostAsync("User", content);
            }
            else
            {
                await _httpClient.PutAsync($"User/{user.Id}", content);
            }

            return RedirectToAction("User_List");
        }

        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"User/{id}");

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("User_List");
                }

                var errorContent = await response.Content.ReadAsStringAsync();
                TempData["ErrorMessage"] = errorContent;
            }
            catch (HttpRequestException ex)
            {
                TempData["ErrorMessage"] = $"Request error: {ex.Message}";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Unhandled error: {ex.Message}";
            }

            return RedirectToAction("User_List");
        }
    }
}