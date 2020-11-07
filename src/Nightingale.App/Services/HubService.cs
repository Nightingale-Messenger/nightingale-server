using System;
using System.Collections.Generic;
using System.Security.Policy;
using System.Threading.Tasks;
using Nightingale.App.Interfaces;
using Nightingale.App.Mapper;
using Nightingale.App.Models;
using Nightingale.Core.Identity;
using Nightingale.Core.Repositories;

namespace Nightingale.App.Services
{
    public class HubService : IHubService
    {
        private readonly IUserService _userService;

        private readonly IMessageService _messageService;

        private readonly IMessageRepository _messageRepository;
        
        public HubService(IUserService userService,
            IMessageService messageService,
            IMessageRepository messageRepository)
        {
            _userService = userService;
            _messageService = messageService;
            _messageRepository = messageRepository;
        }

        public async Task<MessageModel> SendAsync(NewMessageModel msg)
        {
            var sender = await _userService.GetByIdAsync(msg.SenderId);
            var receiver = await _userService.GetByIdAsync(msg.ReceiverId);

            if (sender == null || receiver == null)
            {
                throw new Exception();
            }
            return await _messageService.Create(sender, receiver, msg.Text, msg.Date);
        }

        public async Task<IEnumerable<MessageModel>> GetLastMessagesAsync(string issuerId, string receiverId)
        {
            return await _messageService.GetLastN(20, issuerId, receiverId);
        }

        public Task<IEnumerable<UserModel>> GetContacts(User user)
        {
            throw new System.NotImplementedException();
        }

        public Task<IEnumerable<UserModel>> FindByPublicUserName(string publicUserName)
        {
            throw new System.NotImplementedException();
        }

        public Task<IEnumerable<MessageModel>> GetMessagesBeforeId(int id)
        {
            throw new System.NotImplementedException();
        }
    }
}