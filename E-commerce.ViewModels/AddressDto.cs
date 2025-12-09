using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce.ViewModels
{
    public class AddressDto
    {
        public ShippingAddressDto Shipping { get; set; }
        public BillingAddressDto Billing { get; set; }
    }
    public class ShippingAddressDto
    {
        public string FullName { get; set; }
        public long Phone { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public int PostalCode { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
    }

    public class BillingAddressDto
    {
        public string FullName { get; set;}
        public long Phone { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public int PostalCode { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
    }
}
