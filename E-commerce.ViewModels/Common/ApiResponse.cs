using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce.ViewModels.Common
{
    public class ApiResponse<T> where T : class
    {
        public bool IsSuccess { get; set; } = false;
        public string Message { get; set; } = "Something went wrong.";
        public T Result { get; set; }
        public int Id { get; set; }
    }
}
