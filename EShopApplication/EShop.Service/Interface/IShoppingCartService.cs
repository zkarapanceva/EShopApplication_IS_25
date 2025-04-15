using EShop.Domain.DomainModels;
using EShop.Domain.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Service.Interface
{
    public interface IShoppingCartService
    {
        ShoppingCart GetByUserId(Guid id);
        ShoppingCartDTO GetByUserIdIncudingProducts(Guid id);
        AddToCartDTO GetProductInfo(Guid id);
        Boolean DeleteFromCart(Guid id, string userId);
        Boolean OrderProducts(string userId);
    }
}
