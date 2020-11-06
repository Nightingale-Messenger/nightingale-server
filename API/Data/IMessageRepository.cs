using System.Collections.Generic;
using API.Models;

namespace API.Data
{
    public interface IMessageRepository : IRepository<Message, int>
    {
        public IEnumerable<Message> GetLastN(int n, User issuer, User target);
    }
}