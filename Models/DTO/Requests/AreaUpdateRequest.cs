using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ZooManager.Models.DTO.Requests
{
    public class AreaUpdateRequest
    {
        [Required]
        public string Name { get; set; }
    }
}
