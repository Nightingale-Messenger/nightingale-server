using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Nightingale.Core.Entities;

namespace Nightingale.Core.Identity
{
    public class User : IdentityUser
    {
        public string PublicUSerName { get; set; }
        
        public IEnumerable<Message> Messages;
    }
}