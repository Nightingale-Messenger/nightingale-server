using System.Threading.Tasks;
using API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace API.Hubs
{
    class UserHub : Hub
    {
        [Authorize]
        public async Task Send(string message, string to)
        {
            await Clients.User(to).SendAsync("ReceiveMessage", message);
        }
    }
}