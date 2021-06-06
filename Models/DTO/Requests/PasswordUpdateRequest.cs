using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ZooManager.Models.DTO.Requests
{
    public class PasswordUpdateRequest
    {
        public string CurrentPassword { get; set; }

        [MinLength(6, ErrorMessage = "Password should be at least {0} characters long")]
        public string NewPassword { get; set; }

        [Compare("NewPassword", ErrorMessage = "Passwords should be equal")]
        public string ConfirmNewPassword { get; set; }
    }
}
