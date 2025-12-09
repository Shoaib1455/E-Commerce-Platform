using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce.ViewModels
{
    public class NotificationDto
    {
        public string Title { get; set; }
        public string Message { get; set; }
        public string OrderId { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}
