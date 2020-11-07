using System;
using System.Collections.Generic;
using System.Security.Policy;
using System.Threading.Tasks;
using Nightingale.App.Interfaces;
using Nightingale.App.Mapper;
using Nightingale.App.Models;
using Nightingale.Core.Entities;
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

        public async Task<IEnumerable<UserModel>> GetContacts(User user)
        {
            return await _messageService.GetContacts(user.Id);
        }

        public Task<IEnumerable<UserModel>> FindByPublicUserName(string publicUserName)
        {
            return _userService.FindByPublicUserNameAsync(publicUserName);
        }

        public Task<IEnumerable<MessageModel>> GetMessagesBeforeId(int id)
        {
            return _messageService.GetMessagesBeforeId(id);
        }

        public async Task<bool> CheckMessagePermission(int messageId, string userId)
        {
            var message = await _messageRepository.Find(messageId);

            if (message.SenderId == userId || message.ReceiverId == userId) return true;

            return false;

        }
    }
}