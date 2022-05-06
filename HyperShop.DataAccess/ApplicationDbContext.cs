using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using HyperShop.Models;

namespace HyperShop.DataAccess
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext (DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<HyperShop.Models.Category> Categories { get; set; }
        public DbSet<HyperShop.Models.Brand> Brands { get; set; }
    }
}
