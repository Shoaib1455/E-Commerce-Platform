using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce.ViewModels
{
    public class ProductVM
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        public int Price { get; set; }

        public string? Sku { get; set; }
        public int quantity {  get; set; }
        public bool? Isactive { get; set; }
        public string ImageUrl { get; set; } = null;
        public string CategoryName { get; set; } = null!;
        public DateTime? Createdat { get; set; }

        public DateTime? Updatedat { get; set; }
    }
}
