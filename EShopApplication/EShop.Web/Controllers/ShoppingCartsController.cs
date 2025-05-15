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
using Microsoft.Extensions.Options;
using EShop.Domain;
using Stripe;
using MimeKit.Cryptography;

namespace EShop.Web.Controllers
{
    public class ShoppingCartsController : Controller
    {
        private readonly IShoppingCartService _cartService;

        public ShoppingCartsController(IShoppingCartService cartService, IOptions<StripeSettings> stripeSettings)
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
        public IActionResult PayOrder(string stripeEmail, string stripeToken)
        {

            StripeConfiguration.ApiKey = "<secret_key>";
            var customerService = new Stripe.CustomerService();
            var chargeService = new ChargeService();

            string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var order = _cartService.GetByUserIdIncudingProducts(Guid.Parse(userId));

            var customer = customerService.Create(new CustomerCreateOptions
            {
                Email = stripeEmail,
                Source = stripeToken
            });

            var charge = chargeService.Create(new ChargeCreateOptions
            {
                Amount = (Convert.ToInt32(order.TotalPrice) * 100),
                Description = "EShop Payment",
                Currency = "usd",
                Customer = customer.Id
            });

            if (charge.Status == "succeeded")
            {
                this.Order();
                return RedirectToAction("SuccessPayment");
            }
            else
                return RedirectToAction("NotsuccessPayment");

        }

        public IActionResult SuccessPayment()
        {
            return View(); 
        }

    }
}
