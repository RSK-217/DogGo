using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DogGo.Models;

namespace DogGo.Data
{
    public class DogGoContext : DbContext
    {
        public DogGoContext (DbContextOptions<DogGoContext> options)
            : base(options)
        {
        }

        public DbSet<DogGo.Models.Walker> Walker { get; set; } = default!;
    }
}
