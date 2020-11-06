using System.ComponentModel.DataAnnotations;

namespace Nightingale.App.Models
{
    public class RegisterModel
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        
        [Required]
        public string UserName { get; set; }
    }
}