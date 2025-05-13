using EShop.Domain.DomainModels;
using EShop.Repository;
using EShop.Service.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Service.Implementation
{
    public class OrderService : IOrderService
    {
        private readonly IRepository<Order> _orderRepository;

        public OrderService(IRepository<Order> orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public List<Order> GetAllOrders()
        {
            return _orderRepository.GetAll(selector: x => x,
                include: x => x.Include(y => y.ProductsInOrder)
                .ThenInclude(z => z.Product).Include(z=> z.Owner))
                .ToList();
        }

        public Order GetOrderDetails(Guid Id)
        {
            return _orderRepository.Get(selector: x => x,
                predicate: x => x.Id == Id,
                include: x => x.Include(y => y.ProductsInOrder)
                .ThenInclude(z => z.Product).Include(y => y.Owner));
        }
    }
}
