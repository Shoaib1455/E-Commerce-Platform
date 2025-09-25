using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce.ViewModels
{
    public class OrderitemDto
    {
        public int Id { get; set; }

        public int? Productid { get; set; }

        public int? Quantity { get; set; }

        public int? Unitprice { get; set; }

        public int? Totalprice { get; set; }

        public int? Orderid { get; set; }
    }
}
