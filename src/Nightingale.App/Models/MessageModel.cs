using System;
using Nightingale.Core.Identity;

namespace Nightingale.App.Models
{
    public class MessageModel
    {
        public int Id { get; set; }
        
        //public User Sender { get; set; }
        
        //public string SenderId { get; set; }

        public UserModel Sender { get; set; }
        
        //public User Receiver { get; }
        
        //public string ReceiverId { get; set; }
        
        public UserModel Receiver { get; set; }
        
        public string Text { get; set; }
        
        public DateTime DateTime { get; set; }
    }
}