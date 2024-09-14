using Microsoft.AspNetCore.SignalR;

namespace Authentication.Api.Core.NotificationHub
{
    public class RegisterHub : Hub
    {
        public async Task SendMessage(string message)
        {
            await Clients.All.SendAsync("RegisterMessage", message);
        }
    }
}
