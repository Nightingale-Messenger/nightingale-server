using System.Collections.Generic;

namespace API.Models
{
    public class Contact
    {
        public int Id { get; set; }
        
        public List<Message> Messages { get; set; }
        
        //public ICollection<User> Users { get; set; }
        public List<UserContact> UserContacts { get; set; }
    }
}
