using AdminApplication.Models;
using Microsoft.AspNetCore.Mvc;

namespace AdminApplication.Controllers
{
    public class OrderController : Controller
    {
        public async Task<IActionResult> Index()
        {
            HttpClient client = new HttpClient();

            string URL = "http://localhost:5041/api/admin/getallorders";
            HttpResponseMessage message = await client.GetAsync(URL);
            var data = await message.Content.ReadFromJsonAsync<List<Order>>();
            return View(data);
        }
        public async Task<IActionResult> Details(Guid id)
        {
            HttpClient client = new HttpClient();

            string URL = $"http://localhost:5041/api/admin/GetOrderDetails?id={id}";
            HttpResponseMessage message = await client.PostAsync(URL, null);
            var data = await message.Content.ReadFromJsonAsync<Order>();
            return View(data);
        }
    }
}
