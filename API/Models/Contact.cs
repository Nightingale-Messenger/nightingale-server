using System.Collections.Generic;

namespace API.Models
{
    public class Contact
    {
        public int Id { get; set; }

        public List<User> Users;
    }
}
