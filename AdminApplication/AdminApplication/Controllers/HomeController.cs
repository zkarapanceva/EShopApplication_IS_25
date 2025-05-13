using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using AdminApplication.Models;
using Newtonsoft.Json;
using System.Text;
using ExcelDataReader;
using ClosedXML.Excel;
using GemBox.Document;

namespace AdminApplication.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
        ComponentInfo.SetLicense("FREE-LIMIMTED-KEY");
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    [HttpGet]
    public IActionResult ImportUsers()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> ImportUsers(IFormFile file)
    {
        string pathToUpload = $"{Directory.GetCurrentDirectory()}\\files\\{file.FileName}";
        using (FileStream fileStream = System.IO.File.Create(pathToUpload))
        {
            file.CopyTo(fileStream);
            fileStream.Flush();
        }

        List<UserDTO> users = new List<UserDTO>();

        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

        using (var stream = System.IO.File.Open(pathToUpload, FileMode.Open, FileAccess.Read))
        {
            using (var reader = ExcelReaderFactory.CreateReader(stream))
            {
                while (reader.Read())
                {
                    users.Add(new UserDTO
                    {
                        Username = reader.GetValue(0).ToString(),
                        Password = reader.GetValue(1).ToString(),
                        ConfirmPassword = reader.GetValue(2).ToString()
                    });
                }
            }
        }
        var client = new HttpClient();
        string URL = "http://localhost:5041/api/admin/ImportUsers";
        HttpContent content = new StringContent(JsonConvert.SerializeObject(users), Encoding.UTF8, "application/json");
        HttpResponseMessage response = await client.PostAsync(URL, content);
        var result = await response.Content.ReadAsAsync<bool>();
        return RedirectToAction("Index", "Home");
    }

    public async Task<IActionResult> ExportOrders()
    {
        string fileName = "Order.xlsx";
        string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";


        using (var workbook = new XLWorkbook())
        {
            IXLWorksheet worksheet = workbook.Worksheets.Add("All Orders");

            worksheet.Cell(1, 1).Value = "Order Id";
            worksheet.Cell(1, 2).Value = "Customer Email";

            var client = new HttpClient();
            string URL = "http://localhost:5041/api/admin/getallorders";

            HttpResponseMessage response = await client.GetAsync(URL);

            var result = await response.Content.ReadAsAsync<List<Order>>();

            for (int i = 1; i <= result.Count; i++)
            {
                var currentOrder = result[i - 1];

                worksheet.Cell(i + 1, 1).Value = currentOrder.Id.ToString();
                worksheet.Cell(i + 1, 2).Value = currentOrder.Owner.FirstName;

                for (int j = 0; j < currentOrder.ProductsInOrder.Count(); j++)
                {
                    var currentProductInOrder = currentOrder.ProductsInOrder.ToList().ElementAt(j);

                    worksheet.Cell(1, j + 3).Value = "Product-" + (j + 1);
                    worksheet.Cell(i + 1, j + 3).Value = currentProductInOrder.Product.ProductName;
                }
            }
            using (var stream = new MemoryStream())
            {
                workbook.SaveAs(stream);
                var content = stream.ToArray();
                return File(content, contentType, fileName);
            }
        }

    }

    public async Task<IActionResult> CreateInvoice(Guid id)
    {


        var client = new HttpClient();

        string URL = $"http://localhost:5041/api/admin/GetOrderDetails?id={id}";
        HttpResponseMessage response = await client.PostAsync(URL, null);

        var result = await response.Content.ReadAsAsync<Order>();

        var templatePath = Path.Combine(Directory.GetCurrentDirectory(), "Invoice.docx");
        var document = DocumentModel.Load(templatePath);

        StringBuilder userSb = new StringBuilder();
        userSb.Append(result.Owner.FirstName);
        userSb.Append(" - ");
        userSb.Append(result.Owner.LastName);


        document.Content.Replace("{{OrderNumber}}", result.Id.ToString());
        document.Content.Replace("{{UserName}}", userSb.ToString());

        StringBuilder sb = new StringBuilder();

        var totalPrice = 0.0;

        foreach (var item in result.ProductsInOrder)
        {
            totalPrice += item.Quantity * item.Product.Price;
            sb.AppendLine(item.Product.ProductName + " with quantity of: " + item.Quantity + " and price of: " + item.Product.Price + "$");
        }


        document.Content.Replace("{{ProductList}}", sb.ToString());
        document.Content.Replace("{{TotalPrice}}", totalPrice.ToString() + "$");


        var stream = new MemoryStream();

        document.Save(stream, new PdfSaveOptions());

        return File(stream.ToArray(), new PdfSaveOptions().ContentType, "ExportInvoice.pdf");
    }
}
