using Nightingale.Core.Entities;

namespace Nightingale.API.Models
{
    public class TokensResult
    {
        public string AccessToken { get; set; }
        
        public RefreshToken RefreshToken { get; set; }
    }
}