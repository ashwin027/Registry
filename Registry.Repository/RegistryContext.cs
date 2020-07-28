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
        public DbSet<RegistryRecord> Registry { get; set; }

        public RegistryContext(DbContextOptions<RegistryContext> options) : base(options)
        {
        }
    }
}
