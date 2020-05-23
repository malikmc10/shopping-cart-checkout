using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MileStone2_1.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace MileStone2_1.Models
{
    public class ApplicationDBContext: IdentityDbContext
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> dbContextOptions)
            : base (dbContextOptions)
        { }

        public DbSet<Phone> Phones { get; set; }

        public DbSet<User> User { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //modelBuilder.Seed();

           

        }

    }
}
