using System;

namespace API.Models
{
    public class Message
    {
        public int Id { get; set; }
        
        public string Text { get; set; }
        
        public User Sender { get; set; }

        public User Receiver { get; set; }
        
        public DateTime DateTime { get; set; }
        
        //public Contact Contact { get; set; }

        //public Conversation Conversation { get; set; }
    }
}