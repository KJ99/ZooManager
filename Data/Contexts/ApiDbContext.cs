using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZooManager.Models.Entities;

namespace ZooManager.Data.Contexts
{
    public class ApiDbContext : DbContext
    {
        public ApiDbContext(DbContextOptions<ApiDbContext> options): base(options)
        {
             
        }

        public DbSet<Area> Areas { get; set; }

        public DbSet<Species> Species { get; set; }

        public DbSet<Animal> Animals { get; set; }
    }
}
