using System;

namespace API.Models
{
    public class ChatMessage
    {
        //public Guid SenderId { get; set; }
        
        //public Guid ReceiverId { get; set; }
        
        public int? ContactId { get; set; }
        
        public string? ReceiverId { get; set; }

        public string Text { get; set; }
        
        public DateTime Date { get; set; }
    }
}