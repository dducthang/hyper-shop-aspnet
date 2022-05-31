using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using HyperShop.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace HyperShop.DataAccess
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext (DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<HyperShop.Models.Category> Categories { get; set; }
        public DbSet<HyperShop.Models.Brand> Brands { get; set; }
        public DbSet<HyperShop.Models.Color> Colors { get; set; }
        public DbSet<HyperShop.Models.Size> Sizes { get; set; }
        public DbSet<HyperShop.Models.Product> Products { get; set; }
        public DbSet<HyperShop.Models.Stock> Stock { get; set; }
        public DbSet<HyperShop.Models.PrimaryImage> PrimaryImages { get; set; }
        public DbSet<HyperShop.Models.SecondaryImage> SecondaryImages { get; set; }
        public DbSet<HyperShop.Models.City> Cities { get; set; }
        public DbSet<HyperShop.Models.ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<HyperShop.Models.Cart> Carts { get; set; }
        public DbSet<HyperShop.Models.Order> Orders { get; set; }
        public DbSet<HyperShop.Models.OrderDetail> OrderDetails { get; set; }
        public DbSet<HyperShop.Models.Comment> Comments { get; set; }
        public DbSet<HyperShop.Models.Response> Responses { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>()
                .HasMany<Product>()
                .WithOne(p => p.Category)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Brand>()
                .HasMany<Product>()
                .WithOne(p => p.Brand)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Order>()
                .HasOne<City>()
                .WithMany()
                .HasForeignKey(o=>o.CityName)
                .OnDelete(DeleteBehavior.NoAction);


                base.OnModelCreating(modelBuilder);
        }
    }
}
