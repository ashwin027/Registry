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


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=ProductCatalog;Trusted_Connection=True;");
        }


    }
}
