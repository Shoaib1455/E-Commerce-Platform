﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce.ViewModels
{
    public class UserUpdateVM
    {
        public string Name { get; set; }
        public string Email { get; set; } = null!;
        public string Role { get; set; } = null!;
    }
}
