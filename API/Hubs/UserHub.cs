using System;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace API.Hubs
{
    class UserHub : Hub
    {
        private readonly DatabaseContext _dbContext;
        private readonly UserManager<User> _userManager;

        private UserHub(DatabaseContext db,
            UserManager<User> userManager)
        {
            _dbContext = db;
            _userManager = userManager;
        }
        
        [Authorize]
        public async Task Send(ChatMessage msg)
        {
            var newMessage = new Message()
            {
                DateTime = msg.Date,
                Receiver = await _userManager.FindByIdAsync(Convert.ToString(msg.ReceiverId)),
                Text = msg.Text,
                User = await _userManager.FindByIdAsync(Convert.ToString(msg.SenderId))
            };
            await _dbContext.Messages.AddAsync(newMessage);
            await _dbContext.SaveChangesAsync();
            await Clients.User(Convert.ToString(msg.ReceiverId)).SendAsync("ReceiveMessage", newMessage);
        }
        
        [Authorize]
        public async Task GetLastMessages(string receiverId)
        {
            var receiver = await _userManager.FindByIdAsync(receiverId);
            var currUser = await _userManager.GetUserAsync(Context.User);
            await Clients.Caller.SendAsync("GetMessages",
                _dbContext.Messages.Where(m => m.Receiver == receiver &&
                                               m.User == currUser).Take(20));
        }

        public async Task GetMessagesBefore(string receiverId, int messageId)
        {
            var receiver = await _userManager.FindByIdAsync(receiverId);
            var currUser = await _userManager.GetUserAsync(Context.User);

            await Clients.Caller.SendAsync("GetMessages",
                _dbContext.Messages.Where(m => m.Receiver == receiver &&
                                               m.User == currUser &&
                                               m.Id < messageId).Take(20));
        }
    }
}
