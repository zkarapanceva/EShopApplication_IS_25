using EShop.Domain.DomainModels;
using EShop.Domain.DTO;
using EShop.Domain.Identity;
using EShop.Service.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EShop.Web.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IOrderService orderService;
        private readonly UserManager<EShopApplicationUser> userManager;

        public AdminController(IOrderService orderService, UserManager<EShopApplicationUser> userManager)
        {
            this.orderService = orderService;
            this.userManager = userManager;
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
        [HttpPost("[action]")]
        public bool ImportUsers(List<UserDTO> model)
        {
            bool status = true;

            foreach (var item in model)
            {
                var check = userManager.FindByEmailAsync(item.Username).Result;
                if (check == null)
                {
                    var user = new EShopApplicationUser
                    {
                        FirstName = "Test",
                        LastName = "Test",
                        UserName = item.Username,
                        Email = item.Username,
                        EmailConfirmed = true,
                        UserCart = new ShoppingCart()
                    };
                    var result = userManager.CreateAsync(user, item.Password).Result;
                    status = status & result.Succeeded;
                }
                else
                    continue;
            }

            return status;

        }
    }
}
