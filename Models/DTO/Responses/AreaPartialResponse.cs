using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ZooManager.Models.DTO.Responses
{
    public class AreaPartialResponse
    {
        [Required]
        public string Code { get; set; }

        public string Name { get; set; }
    }
}
