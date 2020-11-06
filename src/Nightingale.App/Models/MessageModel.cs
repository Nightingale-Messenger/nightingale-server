using System;
using Nightingale.Core.Identity;

namespace Nightingale.App.Models
{
    public class MessageModel
    {
        public int Id { get; set; }
        
        public User Sender { get; set; }
        
        public string SenderId { get; }
        
        public User Receiver { get; }
        
        public string ReceiverId { get; }
        
        public string Text { get; }
        
        public DateTime DateTime { get; }
    }
}