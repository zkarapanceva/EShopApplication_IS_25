using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Domain.DTO
{
    public class AddToCartDTO
    {
        public Guid ProductId { get; set; }
        public string? ProductName { get; set; }
        public int Quantity { get; set; }
    }
}
