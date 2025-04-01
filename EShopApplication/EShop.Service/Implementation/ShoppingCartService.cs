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

        public ShoppingCartService(IRepository<ShoppingCart> shoppingCartRepository)
        {
            _shoppingCartRepository = shoppingCartRepository;
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
    }
}
