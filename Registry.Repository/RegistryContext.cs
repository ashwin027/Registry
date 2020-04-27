using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Registry.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Registry.Repository
{
    // Command to add a migration
    // dotnet ef migrations add initial-data --startup-project "..\Registry.UI"

    // Command to update the DB
    // dotnet ef database update  --startup-project "..\Registry.UI"
    public class RegistryContext: DbContext
    {
        private readonly string _connectionString;
        public DbSet<RegistryRecord> Registry { get; set; }

        public RegistryContext(IOptionsMonitor<Config> configAccessor, DbContextOptions<RegistryContext> options) : base(options)
        {
            _connectionString = configAccessor.CurrentValue.ConnectionString;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_connectionString);
        }
    }
}
