using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ZooManager.Models.DTO.Requests
{
    public class AreaRequest
    {
        [Required]
        [MinLength(3, ErrorMessage = "Code should be at least 3 characters long")]
        public string Code { get; set; }

        [Required]
        public string Name { get; set; }
    }
}
