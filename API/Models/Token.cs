using System;

namespace API.Models
{
    public class Token
    {
        public Guid Uuid { get; }
        
        public User User { get; }
    }
}