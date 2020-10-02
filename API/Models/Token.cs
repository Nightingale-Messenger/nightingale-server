using System;

namespace API.Models
{
    public class Token
    {
        public int Id { get; set; }
        public string AccessToken { get; set; }

        public User User { get; set; }
    }
}