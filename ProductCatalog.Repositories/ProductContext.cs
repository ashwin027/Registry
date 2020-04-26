using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using ProductCatalog.Common.Models;
using ProductCatalog.Models;
using ProductCatalog.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductCatalog.Repository
{
    // Command to add a migration
    // dotnet ef migrations add initial-data --startup-project "..\ProductCatalog.Api"

    // Command to update the DB
    // dotnet ef database update  --startup-project "..\ProductCatalog.Api"
    public class ProductContext : DbContext
    {
        private readonly string _connectionString;
        public DbSet<Product> Products { get; set; }
        public ProductContext(IOptionsMonitor<Config> configAccessor, DbContextOptions<ProductContext> options) : base(options)
        {
            _connectionString = configAccessor.CurrentValue.ConnectionString;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_connectionString);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Product>().HasData(new Product { Id=1, Name = "Teether", Description = "Baby Orange - Training Teether Tooth Brush for Infant, Baby, and Toddler" });
            modelBuilder.Entity<Product>().HasData(new Product { Id = 2, Name = "Foo Pacifier value pack", Description = "Pacifier Value Pack, Boy, 6-18 Months (Pack of 3)" });
            modelBuilder.Entity<Product>().HasData(new Product { Id = 3, Name = "Baby fidget cube", Description = "Fisher-Price Fidget Cube" });
            modelBuilder.Entity<Product>().HasData(new Product { Id = 4, Name = "Steps Walker", Description = "Costco steps walker (12 - 18 months)" });
            modelBuilder.Entity<Product>().HasData(new Product { Id = 5, Name = "Summer Booster chair", Description = "Activity floor seat and booster all in one" });
            modelBuilder.Entity<Product>().HasData(new Product { Id = 6, Name = "Bassinet", Description = "Bassinet - 1 to 8 months" });
        }


    }
}
