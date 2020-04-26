using Microsoft.EntityFrameworkCore;
using ProductCatalog.Models;
using ProductCatalog.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductCatalog.Repository
{
    public class ProductContext : DbContext
    {
        private readonly string _connectionString;
        public DbSet<Product> Products { get; set; }
        public ProductContext(Config config, DbContextOptions<ProductContext> options) : base(options)
        {
            _connectionString = config.ConnectionString;
        }

        public ProductContext(DbContextOptions<ProductContext> options) : base(options)
        {
        }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // optionsBuilder.UseSqlServer(_connectionString);
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=ProductCatalog;Trusted_Connection=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Product>().HasData(new Product { Id=1, Name = "Teether", Description = "Baby Orange - Training Teether Tooth Brush for Infant, Baby, and Toddler" });
            modelBuilder.Entity<Product>().HasData(new Product { Id = 2, Name = "Foo Pacifier value pack", Description = "Pacifier Value Pack, Boy, 6-18 Months (Pack of 3)" });
            modelBuilder.Entity<Product>().HasData(new Product { Id = 3, Name = "Baby fidget cube", Description = "Fisher-Price Fidget Cube" });
            modelBuilder.Entity<Product>().HasData(new Product { Id = 4, Name = "Steps Walker", Description = "Costco steps walker (12 - 18 months)" });
            modelBuilder.Entity<Product>().HasData(new Product { Id = 5, Name = "Summer Booster chair", Description = "Activity floor seat and booster all in one" });
        }


    }
}
