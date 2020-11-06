using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.Models;
using API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace API.Hubs
{
    class UserHub : Hub
    {
        public DatabaseContext DbContext { get; }
        public UserManager<User> UserManager { get; }
        public IMessageRepository MessageRepository { get; }

        private UserHub(DatabaseContext db,
            UserManager<User> userManager,
            IMessageRepository messageRepository)
        {
            DbContext = db;
            UserManager = userManager;
            MessageRepository = messageRepository;
        }
        
        [Authorize]
        public async Task Send(ChatMessage msg)
        {
            await this.SendAsync(msg);
        }

        [Authorize]
        public async Task GetLastMessages(string userId)
        {
            await this.GetLastMessagesAsync(userId, 20);
        }
        /*
            [Authorize]
            public async Task GetContacts() 
            {
                var user = await _userManager.GetUserAsync(Context.User);
                await Clients.Caller.SendAsync("GetContacts",
                    _dbContext.Contacts
                        .Where(c => c.Users.Contains(user))
                        .Select(c => new {c.Id,
                            Users = c.Users
                            .Select(u => new {u.Id, u.PublicUserName, u.UserName}).ToArray()}));
            }
    
            [Authorize]
            public async Task FindByPublicNickName(string SearchString)
            {
                var users = _dbContext.Users
                    .Where(x => EF.Functions.Like(x.PublicUserName, SearchString));
                
                await Clients.Caller.SendCoreAsync("RetrieveSearchResults",
                    users.Select(u => new {u.UserName, u.Id, PublicNickName = u.PublicUserName}).ToArray());
            }
    
            [Authorize]
            public async Task GetMessagesBefore(int contactId, int messageId)
            {
                await Clients.Caller.SendAsync("GetMessages",
                    _dbContext.Messages.Where(m => m.Contact.Id == contactId &&
                                                   m.Id < messageId).Take(20));
            }*/
    }
}
