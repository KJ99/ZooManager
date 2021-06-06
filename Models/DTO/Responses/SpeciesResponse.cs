using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZooManager.Models.DTO.Responses
{
    public class SpeciesResponse
    { public int Id { get; set;  }
        public string Name { get; set; }
        public string Description { get; set; }
        public ICollection<AnimalPartialResponse> Animals { get; set; }
    }
}