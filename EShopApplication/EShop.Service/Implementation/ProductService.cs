using EShop.Domain.DomainModels;
using EShop.Domain.DTO;
using EShop.Repository;
using EShop.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Service.Implementation
{
    public class ProductService : IProductService
    {
        private readonly IRepository<Product> _productRepository;
        private readonly IRepository<ProductInShoppingCart> _productInCartsRepository;
        private readonly IShoppingCartService _shoppingCartService;

        public ProductService(IRepository<Product> productRepository, IRepository<ProductInShoppingCart> productInCartsRepository, IShoppingCartService shoppingCartService)
        {
            _productRepository = productRepository;
            _productInCartsRepository = productInCartsRepository;
            _shoppingCartService = shoppingCartService;
        }

        public Product Add(Product product)
        {
            product.Id = Guid.NewGuid();
            return _productRepository.Insert(product);
        }

        public void AddToCart(AddToCartDTO modelDTO, Guid userId)
        {
            var shoppingCart = _shoppingCartService.GetByUserId(userId);
            var product = _productRepository.Get(selector: x => x,
                                                 predicate: x => x.Id == modelDTO.ProductId);

            var existing = _productInCartsRepository.Get(selector: x => x,
                    predicate: x => x.ProductId == modelDTO.ProductId && x.ShoppingCartId == shoppingCart.Id);

            if(existing == null)
            {
                ProductInShoppingCart newProduct = new ProductInShoppingCart
                {
                    Id = Guid.NewGuid(),
                    Product = product,
                    ProductId = modelDTO.ProductId,
                    ShoppingCart = shoppingCart,
                    ShoppingCartId = shoppingCart.Id,
                    Quantity = modelDTO.Quantity
                };
                _productInCartsRepository.Insert(newProduct);
            }
            else
            {
                existing.Quantity += modelDTO.Quantity;
                _productInCartsRepository.Update(existing);
            }
        }

        public Product DeleteById(Guid Id)
        {
            var product = _productRepository.Get(selector: x => x,
                                                predicate: x => x.Id == Id);
            return _productRepository.Delete(product);
        }

        public List<Product> GetAll()
        {
            return _productRepository.GetAll(selector: x => x).ToList();
        }

        public Product? GetById(Guid Id)
        {
            return _productRepository.Get(selector: x => x,
                                            predicate: x => x.Id == Id);
        }

        public Product Update(Product product)
        {
            return _productRepository.Update(product);
        }
    }
}
