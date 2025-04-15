using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EShop.Domain.DomainModels;
using EShop.Repository;
using EShop.Service.Interface;
using System.Security.Claims;

namespace EShop.Web.Controllers
{
    public class ShoppingCartsController : Controller
    {
        private readonly IShoppingCartService _cartService;

        public ShoppingCartsController(IShoppingCartService cartService)
        {
            _cartService = cartService;
        }

        public IActionResult Index()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var cart = _cartService.GetByUserIdIncudingProducts(Guid.Parse(userId));
            return View(cart);
        }
        public IActionResult DeleteFromCart(Guid id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            _cartService.DeleteFromCart(id, userId);
            return RedirectToAction("Index");
        }
        public IActionResult Order()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            _cartService.OrderProducts(userId);
            return RedirectToAction("Index");
        }
    }
}
