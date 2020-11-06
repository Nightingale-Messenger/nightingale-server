using System.Threading.Tasks;
using API.Hubs;
using API.Models;
using Microsoft.AspNetCore.SignalR;

namespace API.Services
{
    public static class HubService
    {
        internal static async Task SendAsync(this UserHub hubContext, ChatMessage msg)
        {
            var receiver = await hubContext.UserManager.FindByIdAsync(msg.ReceiverId);
            var user = await hubContext.UserManager.GetUserAsync(hubContext.Context.User);

            var newMessage = new Message()
            {
                DateTime = msg.Date,
                Sender = user,
                Receiver = receiver,
                Text = msg.Text,
            };
            //await _dbContext.Messages.AddAsync(newMessage);

            await hubContext.MessageRepository.Add(newMessage);

            await hubContext.DbContext.SaveChangesAsync();

            await hubContext.Clients.User(receiver.Id).SendAsync("ReceiveMessage", new
            {
                Time = msg.Date,
                Sender = user.Id,
                Text = msg.Text
            });
        }

        internal static async Task GetLastMessagesAsync(this UserHub hubContext, string userId, int n)
        {
            var user = await hubContext.UserManager.GetUserAsync(hubContext.Context.User);
            var target = await hubContext.UserManager.FindByIdAsync(userId);
            await hubContext.Clients.Caller.SendAsync("GetMessages",
                hubContext.MessageRepository.GetLastN(n, user, target));
        }
    }
}