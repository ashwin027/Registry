using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;

namespace Registry.Repository
{
    public class RegistryRepository : IRegistryRepository
    {
        private readonly ILogger<RegistryRepository> _logger;
        private readonly RegistryContext _registryDbContext;
        private const int defaultPageIndex = 1;
        private const int defaultPageSize = 5;
        public RegistryRepository(RegistryContext dbContext, ILogger<RegistryRepository> logger)
        {
            _registryDbContext = dbContext;
            _logger = logger;
        }

        public async Task<Models.RegistryRecord> AddToRegistry(Models.RegistryRecord registry)
        {
            try
            {
                _registryDbContext.Registry.Add(registry);
                await _registryDbContext.SaveChangesAsync();

                return registry;
            }
            catch (Exception ex)
            {
                _logger.LogError("Server Error in AddToRegistry().", ex);
                throw ex;
            }
        }

        // TODO: Update method to use pagination
        public async Task<List<Models.RegistryRecord>> GetRegistryForUser(int userId)
        {
            try
            {
                var registry = await _registryDbContext.Registry.Where(reg => reg.UserId == userId).ToListAsync();
                return registry;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task RemoveFromRegistry(int? id)
        {
            try
            {
                var registryRecord = await _registryDbContext.Registry.FindAsync(id);
                _registryDbContext.Registry.Remove(registryRecord);
                await _registryDbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError("Server Error in RemoveFromRegistry().", ex);
                throw ex;
            }
        }

        public async Task<Models.RegistryRecord> UpdateRegistry(Models.RegistryRecord registry)
        {
            try
            {
                if (registry?.Id == null)
                {
                    return null;
                }

                var existingRegistryRecord = await _registryDbContext.Registry.FindAsync(registry.Id);
                if (existingRegistryRecord == null)
                {
                    return null;
                }

                existingRegistryRecord.Id = registry.Id;
                existingRegistryRecord.ProductId = registry.ProductId;
                existingRegistryRecord.Quantity = registry.Quantity;
                existingRegistryRecord.UserId = registry.UserId;
                await _registryDbContext.SaveChangesAsync();

                return existingRegistryRecord;
            }
            catch (Exception ex)
            {
                _logger.LogError("Server Error in UpdateRegistry().", ex);
                throw ex;
            }
        }
    }
}
