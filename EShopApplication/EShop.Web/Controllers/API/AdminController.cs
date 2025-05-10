using EShop.Domain.DomainModels;
using EShop.Service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace EShop.Web.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IOrderService orderService;

        public AdminController(IOrderService orderService)
        {
            this.orderService = orderService;
        }
        [HttpGet("[action]")]
        public List<Order> GetAllOrders()
        {
            return orderService.GetAllOrders();
        }
        [HttpPost("[action]")]
        public Order GetOrderDetails(Guid id)
        {
            return orderService.GetOrderDetails(id);
        }
    }
}
