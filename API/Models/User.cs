using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models
{
    public class User : IdentityUser
    {
        public string PublicUserName { get; set; }
        //public string AvatarPath { get; set; }

        //public ICollection<Token> Tokens { get; set; }
    }
}