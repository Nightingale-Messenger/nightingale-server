namespace API.Models
{
    public class UserContact
    {
        public string UserId { get; set; }
        public User User { get; set; }
        
        public int ContactId { get; set; }
        public Contact Contact { get; set; }
    }
}
