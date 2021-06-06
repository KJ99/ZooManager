using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ZooManager.Models.Entities
{
    public class Area
    {
        [Key]
        public string Code { get; set; }

        public string Name { get; set; }

        public ICollection<Animal> Animals { get; set; }

    }
}
