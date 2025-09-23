using E_commerce.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce.ViewModels
{
    public class CartdetailsVM
    {
        public Cart carts { get; set; }
        public List<Cartitem> cartitems { get; set; }
    }
}
