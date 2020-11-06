using System.Collections.Generic;
using System.Threading.Tasks;
using Nightingale.App.Models;
using Nightingale.Core.Entities;
using Nightingale.Core.Identity;

namespace Nightingale.App.Interfaces
{
    public interface IMessageService
    {
        Task<IEnumerable<MessageModel>> GetLastN(int n, User Issuer, User Target);
    }
}