using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ProductCatalog.Models;
using ProductCatalog.Models.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProductCatalog.Repository
{
    public class ProductContext: DbContext
    {
        private readonly string _connectionString;
        public DbSet<Product> Products { get; set; }
        public ProductContext(Config config)
        {
            _connectionString = config.ConnectionString;
        }
        

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_connectionString);
        }


    }
}
