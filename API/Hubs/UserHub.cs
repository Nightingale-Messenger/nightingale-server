using System;
using System.Collections.Generic;
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
            if (!msg.ContactId.HasValue &&
                msg.ReceiverId == null)
            {
                throw new NotImplementedException();
            }
            
            var user = await _userManager.GetUserAsync(Context.User);

            User? receiver = null;

            Contact? contact = null;

            if (msg.ReceiverId == null)
            {
                contact = await _dbContext.Contacts.FindAsync(msg.ContactId);
                receiver = contact.Users.FirstOrDefault(u => u.Id != user.Id);
            }
            else
            {
                receiver = await _userManager.FindByIdAsync(Convert.ToString(msg.ReceiverId));
                contact = await _dbContext.Contacts.FirstOrDefaultAsync(c => c.Users.Contains(user) &&
                    c.Users.Contains(receiver));
                if (contact == null)
                {
                    contact = new Contact()
                    {
                        Users = new List<User>() {user, receiver}
                    };
                    await _dbContext.Contacts.AddAsync(contact);
                }
            }

            var newMessage = new Message()
            {
                DateTime = msg.Date,
                Sender = user,
                Text = msg.Text,
                Contact = contact
            };
            
            await _dbContext.Messages.AddAsync(newMessage);

            await _dbContext.SaveChangesAsync();

            Clients.User(receiver.Id).SendAsync("ReceiveMessage", new
            {
                Time = msg.Date,
                Sender = user.Id,
                Contact = contact.Id,
                Text = msg.Text 
            });

            //Clients.Caller.SendCoreAsync("ConfirmMessage", new[] {new {Date = msg.Date.ToString(), contact.Id}});
        }

        [Authorize]
        public async Task GetLastMessages(int contactId)
        {
            //var contact = await _dbContext.Contacts.FindAsync(contactId);
            await Clients.Caller.SendAsync("GetMessages",
                _dbContext.Messages.Where(m => m.Contact.Id == contactId).Take(20));
        }
    
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
        }
    }
}
