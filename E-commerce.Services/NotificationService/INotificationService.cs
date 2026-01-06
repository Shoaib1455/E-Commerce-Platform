using E_commerce.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce.Services.NotificationService
{
    public interface INotificationService
    {
        Task SendToUserAsync(int userId, NotificationDto notification);
    }
}

