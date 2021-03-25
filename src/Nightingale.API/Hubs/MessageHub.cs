using System;
using System.Linq;
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

        public MessageHub(IHubService hubService,
            IUserService userService)
        {
            _hubService = hubService;
            _userService = userService;
        }

        public override async Task OnConnectedAsync()
        {
            var user = await _userService.GetUserAsync(Context.User);
            Console.WriteLine($"User {user.UserName} connected");
            await base.OnConnectedAsync();
        }

        public async Task Send(NewMessageModel msg)
        {
            var user = await _userService.GetUserAsync(Context.User);

            msg.SenderId = Context.UserIdentifier;
            if (msg.SenderId == null)
                return;
            Console.WriteLine(
                $"new message\nsender: {msg.SenderId}\nreceiver: {msg.ReceiverId}\nTime: {DateTime.Now.ToString()}");

            var message = await _hubService.SendAsync(msg);

            await Clients.User(msg.ReceiverId).SendAsync("ReceiveMessage",
                new
                {
                    message.Sender, message.Receiver,
                    message.Id, message.Text, message.DateTime
                });

            await Clients.User(msg.SenderId).SendAsync("ReceiveMessage",
                new
                {
                    message.Sender, message.Receiver,
                    message.Id, message.Text, message.DateTime
                });
        }

        public async Task GetLastMessages(string receiverId)
        {
            var user = await _userService.GetUserAsync(Context.User);
            var messages = await _hubService.GetLastMessagesAsync(user.Id, receiverId);
            // Console.WriteLine(String.Join(',', messages.Select(m => m.Id)));
            await Clients.Caller.SendAsync("GetMessages",
                messages);
        }

        public async Task GetContacts()
        {
            var user = await _userService.GetUserAsync(Context.User);
            await Clients.Caller.SendAsync("GetContacts",
                await _hubService.GetContacts(user));
        }

        public async Task FindByUserName(string publicUserName)
        {
            await Clients.Caller.SendAsync("GetFindResults",
                await _hubService.FindByPublicUserName(publicUserName));
        }

        public async Task GetMessagesBeforeId(int id)
        {
            Console.WriteLine(id);
            if (!await _hubService.CheckMessagePermission(id,
                Context.User.FindFirstValue(ClaimTypes.NameIdentifier)))
            {
                await Clients.Caller.SendAsync("ReportError",
                    $"User have no access to message with id {id}");
                return;
            }

            var res = await _hubService.GetMessagesBeforeId(id);
            // Console.WriteLine(String.Join(',', res.Select(res => res.Id).ToArray()));
            await Clients.Caller.SendAsync("GetMessages",
                res);
        }
    }
}