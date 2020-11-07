using System;

namespace Nightingale.App.Models
{
    public class NewMessageModel
    {
        public string SenderId { get; set; }
        
        public string ReceiverId { get; set; }
        
        public DateTime Date { get; set; }
        
        public string Text { get; set; }
    }
}