using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce.ViewModels
{
    
    public class ForgotPasswordVM
    {
        public string Email { get; set; }
    }

    public class ResetPasswordVM
    {
        public string Email { get; set; }
        public string NewPassword { get; set; }
    }
}
