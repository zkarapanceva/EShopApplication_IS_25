using AdminApplication.Models;
using Microsoft.AspNetCore.Mvc;

namespace AdminApplication.Controllers
{
    public class OrderController : Controller
    {
        public IActionResult Index()
        {
            HttpClient client = new HttpClient();

            string URL = "http://localhost:5041/api/admin/getallorders";
            HttpResponseMessage message = client.GetAsync(URL).Result;
            var data = message.Content.ReadFromJsonAsync<List<Order>>().Result;
            return View(data);
        }
        public async Task<IActionResult> Details(Guid id)
        {
            HttpClient client = new HttpClient();

            string URL = $"http://localhost:5041/api/admin/GetOrderDetails?id={id}";
            HttpResponseMessage message = client.PostAsync(URL, null).Result;
            var data =  message.Content.ReadFromJsonAsync<Order>().Result;
            return View(data);
        }
    }
}
