using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using ZooManager.Attributes;


//Spieces - Ania
//Animals - Piotrek :) <- debil
//Areas - Maciek

/*
    let dupa = (kupa: number) => {
        return kupa * 2
    }
*/


namespace ZooManager.Models.DTO.Requests
{
    public class UserRequest
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Username is required")]
        public string Username { get; set; }

        [EmailAddress(ErrorMessage = "Email address is not valid")]
        public string Email { get; set; }

        [MinLength(6, ErrorMessage = "Password should be at least {0} characters long")]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "Passwords should be equal")]
        public string ConfirmPassword { get; set; }

        [UserRole(ErrorMessage = "At least one of provided roles is not valid")]
        public string[] Roles { get; set; }
    }
}
