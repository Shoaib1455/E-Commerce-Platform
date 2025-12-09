using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce.ViewModels
{
    public class OrderRequestDto
    {
        public AddressDto Address { get; set; }
        public OrderDto Order { get; set; }
        public List<OrderitemDto> OrderItems { get; set; }

        //public AddressDto Address { get; set; }
        //public OrderDto Order { get; set; }
        //public List<OrderitemDto> OrderItems { get; set; }
    }
    
}
