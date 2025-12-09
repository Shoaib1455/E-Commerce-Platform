using E_commerce.Services.SignalR;
using E_commerce.ViewModels;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce.Services.NotificationService
{
    public class NotificationService : INotificationService
    {
        private readonly IHubContext<NotificationHub> _hubContext;
        public NotificationService() { }
        public async Task SendToUserAsync(string userId, NotificationDto notification)
        {
            await _hubContext.Clients.Group(userId)
                .SendAsync("ReceiveNotification", notification);
        }
    }
}
