using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZooManager.Models.DTO.Responses
{
    public class AnimalPartialResponse
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int SpeciesId { get; set; }
        public string AreaCode { get; set; }
    }
}
