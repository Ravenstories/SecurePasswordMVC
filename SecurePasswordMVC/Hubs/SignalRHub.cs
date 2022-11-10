using Microsoft.AspNetCore.SignalR;

namespace SecurePasswordMVC.Hubs
{
    public class SignalRHub : Hub
    {
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }

        public async Task StartEncrypt(object publicKey)
        {
            //await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
    }
}
