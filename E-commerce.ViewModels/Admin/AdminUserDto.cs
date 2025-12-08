using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce.ViewModels.Admin
{
    public class AdminUserDto
    {
        public string? Status { get; set; }      // active, blocked, pending
        public string? Role { get; set; }        // admin, customer, seller
        public string? Search { get; set; }      // name/email/phone
        public int Page { get; set; } = 1;       // default = 1
        public int PageSize { get; set; } = 20;  // default = 20
    }
}
