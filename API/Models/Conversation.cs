using System.Collections.Generic;

namespace API.Models
{
    public class Conversation
    {
        public int Id { get; }
        
        public List<Message> Messages { get; set; }
    }
}