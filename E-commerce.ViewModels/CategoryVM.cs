using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce.ViewModels
{
    public class CategoryVM
    {
        public int Id { get; set; }
        public string? Name { get; set; }

        public string? Description { get; set; }

        public bool? Isactive { get; set; }
    }
}
