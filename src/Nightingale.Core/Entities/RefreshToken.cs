using System;
using Nightingale.Core.Entities.Base;
using Nightingale.Core.Identity;

namespace Nightingale.Core.Entities
{
    public class RefreshToken : Entity
    {
        public User User { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
        public DateTime ExpirationDate { get; set; }
    }
}