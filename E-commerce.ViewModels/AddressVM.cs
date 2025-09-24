using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce.ViewModels
{
    public class AddressVM
    {
        public int Id { get; set; }
        public string Street { get; set; } = null!;

        public string City { get; set; } = null!;

        public string? State { get; set; }

        public int? Postalcode { get; set; }

        public string Country { get; set; } = null!;

        public bool? Isdefault { get; set; }
    }
}
