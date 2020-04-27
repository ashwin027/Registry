using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Registry.Repository
{
    public interface IRegistryRepository
    {
        Task<List<Models.RegistryRecord>> GetRegistryForUser(int userId);
        Task<Models.RegistryRecord> AddToRegistry(Models.RegistryRecord registry);
        Task<Models.RegistryRecord> UpdateRegistry(Models.RegistryRecord registry);
        Task RemoveFromRegistry(int id);
    }
}
