using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using ZooManager.Attributes;

namespace ZooManager.Models.DTO.Requests
{
    public class UserUpdateRequest
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Username is required")]
        public string Username { get; set; }

        [EmailAddress(ErrorMessage = "Email address is not valid")]
        public string Email { get; set; }

        [UserRole(ErrorMessage = "At least one of provided roles is not valid")]
        public string[] RolesToGrant { get; set; }

        [UserRole(ErrorMessage = "At least one of provided roles is not valid")]
        public string[] RolesToRevoke { get; set; }
    }
}
