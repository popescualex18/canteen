using Microsoft.AspNetCore.SignalR.Client;
using SCNeagtovo.BusinessLogic.Interfaces;
using SCNeagtovo.DataModels.Models;

namespace SCNeagtovo.Api.Infrastructure
{
    public class SignalRClientService
    {
        private readonly HubConnection _hubConnection;
        private readonly IBaseBusinessService<Client> _clientBusinessService;
        public SignalRClientService(IServiceScope scope)
        {
            _hubConnection = new HubConnectionBuilder()
                .WithUrl("https://localhost:44359/registerHub")
                .Build();

            _hubConnection.On<string>("RegisterMessage", (message) =>
            {
                // Handle the message
                Console.WriteLine($"Message received: {message}");
            });
            _clientBusinessService = scope.ServiceProvider.GetRequiredService<IBaseBusinessService<Client>>();
        }

        public async Task StartAsync()
        {
            //await _hubConnection.StartAsync();
        }

        public async Task StopAsync()
        {
            await _hubConnection.StopAsync();
        }
    }

}
