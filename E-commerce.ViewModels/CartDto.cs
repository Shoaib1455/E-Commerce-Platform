using E_commerce.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce.ViewModels
{
    public class CartDto
    {
        public int Id { get; set; }

        public int? Userid { get; set; }

        public string? Status { get; set; }

        public DateTime? Createdat { get; set; }

        public DateTimeOffset? Updatedat { get; set; }

        public bool? Isactive { get; set; }

        public virtual ICollection<CartitemDto> Cartitems { get; set; } = new List<CartitemDto>();

        public virtual Usermanagement? User { get; set; }
    }
}
