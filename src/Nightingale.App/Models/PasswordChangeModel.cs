using System.ComponentModel.DataAnnotations;

namespace Nightingale.App.Models
{
    public class PasswordChangeModel
    {
        [Required]
        [DataType(DataType.Password)]
        public string OldPassword { get; set; }
        
        [Required]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }
    }
}