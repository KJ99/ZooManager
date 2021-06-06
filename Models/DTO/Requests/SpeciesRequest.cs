using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ZooManager.Models.DTO.Requests
{
    public class SpeciesRequest
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Species is required")]
        public string Name { get; set; }
        
        public string Description { get; set; }

    }
}
