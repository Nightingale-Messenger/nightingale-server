using System.Collections;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Nightingale.Core.Entities;

namespace Nightingale.Core.Identity
{
    public class User : IdentityUser
    {
        public string PublicUserName { get; set; }
        
        public IEnumerable<Message> SentMessages { get; set; }
        
        public IEnumerable<Message> ReceivedMessages { get; set; }
    }
}