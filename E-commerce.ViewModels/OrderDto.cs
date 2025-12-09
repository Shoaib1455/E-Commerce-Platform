using E_commerce.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce.ViewModels
{
    public class OrderDto
    {
        public int Id { get; set; }

        public int? Userid { get; set; }

        public int? TotalAmount { get; set; }
        public int ShippingFee {  get; set; }
        public string PaymentMethod {  get; set; }

        public DateTime? Createdat { get; set; }

        public DateTime? Updatedat { get; set; }

        public string? Status { get; set; }

        public virtual ICollection<OrderitemDto> Orderitem { get; set; } = new List<OrderitemDto>();
    }
}
