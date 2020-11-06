using System.Collections.Generic;
using System.Threading.Tasks;
using Nightingale.App.Interfaces;
using Nightingale.App.Models;
using Nightingale.Core.Identity;
using Nightingale.Core.Repositories;
using Nightingale.App.Mapper;

namespace Nightingale.App.Services
{
    public class MessageService : IMessageService
    {
        private readonly IMessageRepository _messageRepository;

        public MessageService(IMessageRepository messageRepository)
        {
            _messageRepository = messageRepository;
        }

        public async Task<IEnumerable<MessageModel>> GetLastN(int n, User Issuer, User Target)
        {
            return NightingaleMapper.Mapper.Map<IEnumerable<MessageModel>>(
                _messageRepository.GetLastN(n, Issuer, Target));
        }
    }
}