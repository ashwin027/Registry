using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Reviews.Models;
using Reviews.Models.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Reviews.Repository
{
    // Command to add a migration
    // dotnet ef migrations add initial-data --startup-project "..\Reviews.Api"

    // Command to update the DB
    // dotnet ef database update  --startup-project "..\Reviews.Api"
    public class ReviewContext: DbContext
    {
        private readonly string _connectionString;

        public DbSet<Review> Reviews { get; set; }
        public ReviewContext(IOptionsMonitor<Config> configAccessor, DbContextOptions<ReviewContext> options) : base(options)
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

            modelBuilder.Entity<Review>().HasData(new Review { Id = 1, ProductId = 1, Description = "Great Product!",Title = "Perfect!", Rating = 5, SubmitDate = DateTime.Today.ToUniversalTime()  });
            modelBuilder.Entity<Review>().HasData(new Review { Id = 2, ProductId = 2, Description = "This is a great pacifier.", Title = "Good product!", Rating = 4, SubmitDate = DateTime.Today.ToUniversalTime() });
            modelBuilder.Entity<Review>().HasData(new Review { Id = 3, ProductId = 3, Description = "The cube started to break after the first week of use.", Title = "Terrible product!", Rating = 1, SubmitDate = DateTime.Today.ToUniversalTime() });
            modelBuilder.Entity<Review>().HasData(new Review { Id = 4, ProductId = 1, Description = "This teether worked well for my toddler.", Title = "Ok product", Rating = 3, SubmitDate = DateTime.Today.ToUniversalTime() });
            modelBuilder.Entity<Review>().HasData(new Review { Id = 5, ProductId = 2, Description = "This is a decent product and worked well.", Title = "Decent pacifier.", Rating = 5, SubmitDate = DateTime.Today.ToUniversalTime() });
        }
    }
}
