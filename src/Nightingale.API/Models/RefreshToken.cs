using System;

namespace Nightingale.API.Models
{
    public class RefreshTokenold
    {
        public string Email { get; set; }
        public string Token { get; set; }
        public DateTime ExpirationDate { get; set; }
    }
}