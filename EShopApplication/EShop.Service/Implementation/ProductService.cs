using EShop.Domain.DomainModels;
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

        public ProductService(IRepository<Product> productRepository)
        {
            _productRepository = productRepository;
        }

        public Product Add(Product product)
        {
            product.Id = Guid.NewGuid();
            return _productRepository.Insert(product);
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
