using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZooManager.Models.DTO.Responses
{
    public class UserResponse
    {
        public string Id { get; set; }

        public string Username { get; set; }

        public string Email { get; set; }

        public IList<string> Roles { get; set; }
    }
}
