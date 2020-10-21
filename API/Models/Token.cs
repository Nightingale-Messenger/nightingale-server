using System;

namespace API.Models
{
    public class Token
    {
        public string AccessToken { get; set; }

        public string UserName { get; set; }
        
        public string Email { get; set; }
    }
}