using EShop.Domain;
using EShop.Domain.DomainModels;
using EShop.Domain.DTO;
using EShop.Repository;
using EShop.Service.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Service.Implementation
{
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly IRepository<ShoppingCart> _shoppingCartRepository;
        private readonly IRepository<Product> _productRepository;
        private readonly IRepository<ProductInShoppingCart> _productsInCartsRepository;
        private readonly IRepository<Order> _orderRepository;
        private readonly IRepository<ProductInOrder> _productsInOrderRepository;
        private readonly IEmailService _emailService;

        public ShoppingCartService(IRepository<ShoppingCart> shoppingCartRepository, IRepository<Product> productRepository, IRepository<ProductInShoppingCart> productsInCartsRepository, IRepository<Order> orderRepository, IRepository<ProductInOrder> productsInOrderRepository, IEmailService emailService)
        {
            _shoppingCartRepository = shoppingCartRepository;
            _productRepository = productRepository;
            _productsInCartsRepository = productsInCartsRepository;
            _orderRepository = orderRepository;
            _productsInOrderRepository = productsInOrderRepository;
            _emailService = emailService;

        }

        public bool DeleteFromCart(Guid id, string userId)
        {
            var shoppingCart = _shoppingCartRepository.Get(selector: x => x,
                                                predicate: x => x.OwnerId == userId);
            var productToDelete = _productsInCartsRepository.Get(selector: x => x,
                predicate: x => x.ProductId == id && x.ShoppingCart == shoppingCart);

            _productsInCartsRepository.Delete(productToDelete);
            return true;
        }

        public ShoppingCart GetByUserId(Guid id)
        {
            return _shoppingCartRepository.Get(selector: x => x,
                                                predicate: x => x.OwnerId == id.ToString());
        }

        public ShoppingCartDTO GetByUserIdIncudingProducts(Guid id)
        {
            var shoppingCart = _shoppingCartRepository.Get(selector: x => x,
                    predicate: x => x.OwnerId == id.ToString(),
                    include: x => x.Include(y=> y.ProductInShoppingCarts).ThenInclude(z => z.Product));

            var allProducts = shoppingCart.ProductInShoppingCarts.ToList();

            var allProductPrices = allProducts.Select(x => new
            {
                ProductPrice = x.Product.Price,
                Quantity = x.Quantity
            });
            double total = 0.0;
            foreach(var item in allProductPrices)
            {
                total += item.Quantity * item.ProductPrice;
            }
            ShoppingCartDTO model = new ShoppingCartDTO
            {
                Products = allProducts,
                TotalPrice = total
            };
            return model;
        }

        public AddToCartDTO GetProductInfo(Guid id)
        {
            var product = _productRepository.Get(selector: x => x,
                predicate: x => x.Id == id);
            var dto = new AddToCartDTO
            {
                ProductId = id,
                ProductName = product.ProductName,
                Quantity = 1
            };

            return dto;
        }

        public bool OrderProducts(string userId)
        {
            var shoppingCart = _shoppingCartRepository.Get(selector: x => x,
                    predicate: x => x.OwnerId == userId,
                    include: x => x.Include(y => y.ProductInShoppingCarts).ThenInclude(z => z.Product)
                    .Include(y => y.Owner));

            var newOrder = new Order
            {
                Id = Guid.NewGuid(),
                Owner = shoppingCart.Owner,
                OwnerId = userId
            };
            _orderRepository.Insert(newOrder);

            EmailMessage message = new EmailMessage();
            message.Subject = "Successfull order";
            message.MailTo = shoppingCart.Owner.Email;
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Your order is completed. The order conatins: ");

            var productsInOrder = shoppingCart.ProductInShoppingCarts.Select(z => new ProductInOrder
            {
                Product = z.Product,
                ProductId = z.ProductId,
                Order = newOrder,
                OrderId = newOrder.Id,
                Quantity = z.Quantity
            });

            var total = 0.0;

            foreach(var product in productsInOrder)
            {
                total += (product.Quantity * product.Product.Price);
                _productsInOrderRepository.Insert(product);
                sb.AppendLine(product.Product.ProductName + " with quantity of: " + product.Quantity + " and price of: $" + product.Product.Price);
            }

            sb.AppendLine("Total price for your order: " + total.ToString());
            message.Content = sb.ToString();

            shoppingCart.ProductInShoppingCarts.Clear();
            _shoppingCartRepository.Update(shoppingCart);
            _emailService.SendEmailAsync(message);
            return true;


        }
    }
}
