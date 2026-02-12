using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce.ViewModels
{
    public class ProductImagesVM
    {
        public int? Productid { get; set; }

        public string Imageurl { get; set; } = null!;

        public bool Isprimary { get; set; }

        public DateTimeOffset? Createdat { get; set; }
    }
}
