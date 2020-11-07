using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Nightingale.App.Interfaces;
using Nightingale.App.Models;

namespace Nightingale.API.Hubs
{
    [Authorize]
    public class MessageHub : Hub
    {
        private readonly IHubService _hubService;
        private readonly IUserService _userService;

        private MessageHub(IHubService hubService,
            IUserService userService)
        {
            _hubService = hubService;
            _userService = userService;
        }

        public async Task Send(NewMessageModel msg)
        {
            var user = await _userService.GetUserAsync(Context.User);

            if (msg.SenderId != user.Id)
            {
                await Clients.Caller.SendAsync("ReportError", "Sender id not equal to current user id");
                return;
            }

            var message = await _hubService.SendAsync(msg);

            await Clients.User(msg.ReceiverId).SendAsync("ReceiveMessage", message);
        }

        public async Task GetLastMessages(string receiverId)
        {
            var user = await _userService.GetUserAsync(Context.User);
            await Clients.Caller.SendAsync("GetMessages",
                await _hubService.GetLastMessagesAsync(user.Id, receiverId));
        }

        public async Task GetContacts()
        {
            var user = await _userService.GetUserAsync(Context.User);
            await Clients.Caller.SendAsync("GetContacts",
                await _hubService.GetContacts(user));
        }

        public async Task FindByPublicUserName(string publicUserName)
        {
            await Clients.Caller.SendAsync("GetFindResults",
                await _hubService.FindByPublicUserName(publicUserName));
        }

        public async Task GetMessagesBeforeId(int id)
        {
            if (!await _hubService.CheckMessagePermission(id,
                Context.User.FindFirstValue(ClaimTypes.NameIdentifier)))
            {
                await Clients.Caller.SendAsync("ReportError", 
                    $"User have no access to message with id {id}");
                return;
            }

            await Clients.Caller.SendAsync("GetMessages",
                await _hubService.GetMessagesBeforeId(id));
        }
    }
}