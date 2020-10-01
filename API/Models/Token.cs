using System;

namespace API.Models
{
    public class Token
    {
        public string AccessToken { get; set; }

        public User User { get; set; }
    }
}