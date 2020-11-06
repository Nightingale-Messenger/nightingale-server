using System;
using Nightingale.Core.Entities.Base;
using Nightingale.Core.Identity;

namespace Nightingale.Core.Entities
{
    public class Message : Entity
    {
        public User Sender { get; }
        
        public string SenderId { get; }
        
        public User Receiver { get; }
        
        public string ReceiverId { get; }
        
        public string Text { get; }
        
        public DateTime DateTime { get; }
    }
}