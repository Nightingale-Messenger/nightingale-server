using System.Collections.Generic;

namespace API.Models
{
    public class Conversation
    {
        public int Id { get; set; }
        
        public List<Message> Messages { get; set; }
    }
}