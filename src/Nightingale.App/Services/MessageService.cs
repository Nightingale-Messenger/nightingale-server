using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Nightingale.App.Interfaces;
using Nightingale.App.Models;
using Nightingale.Core.Repositories;
using Nightingale.App.Mapper;
using Nightingale.Core.Entities;

namespace Nightingale.App.Services
{
    public class MessageService : IMessageService
    {
        private readonly IMessageRepository _messageRepository;

        public MessageService(IMessageRepository messageRepository)
        {
            _messageRepository = messageRepository;
        }

        public async Task<MessageModel> Create(UserModel sender, UserModel receiver, string text, DateTime dateTime)
        {
            var msg = new Message()
            {
                //Sender = sender,
                SenderId = sender.Id,
                //Receiver = receiver,
                ReceiverId = receiver.Id,
                Text = text,
                DateTime = dateTime
            };
            await _messageRepository.Add(msg);
            await _messageRepository.Save();
            return NightingaleMapper.Mapper.Map<MessageModel>(msg);
        }

        public async Task<IEnumerable<MessageModel>> GetLastN(int n, string issuerId, string targetId)
        {
            return NightingaleMapper.Mapper.Map<IEnumerable<MessageModel>>(
                _messageRepository.GetLastN(n, issuerId, targetId));
        }
    }
}