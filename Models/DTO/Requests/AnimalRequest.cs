using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;




namespace ZooManager.Models.DTO.Requests
{
    public class AnimalRequest
    {

        [Required]
        public string Name { get; set; }

        [Required]
        public string AreaCode { get; set; }

        [Required]
        public int SpeciesId { get; set; }
    }
}
