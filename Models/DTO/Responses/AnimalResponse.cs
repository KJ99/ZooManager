using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZooManager.Models.DTO.Responses
{
    public class AnimalResponse
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public AreaPartialResponse Area { get; set; }

        public SpeciesPartialResponse Species { get; set; }

    }

}
