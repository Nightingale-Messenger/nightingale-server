using System;
using Nightingale.Core.Entities.Base;
using Nightingale.Core.Identity;

namespace Nightingale.Core.Entities
{
    public class Message : Entity
    {
        public User Sender { get; set; }
        
        public string SenderId { get; set;}
        
        public User Receiver { get; set;}
        
        public string ReceiverId { get; set;}
        
        public string Text { get; set;}
        
        public DateTime DateTime { get; set;}
    }
}